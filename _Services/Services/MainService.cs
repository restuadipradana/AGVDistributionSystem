using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

using AGVDistributionSystem._Services.Interfaces;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Helpers;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.Data;

namespace AGVDistributionSystem._Services.Services
{
    public class MainService : IMainService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public MainService(DataContext context, IMapper mapper, MapperConfiguration configMapper)
        {
            _context = context;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<DataTablesResponse<V_PO2DTO>> POListSearch(DataTablesRequest ListPO)
        {
            var result = new DataTablesResponse<V_PO2DTO>()
            {
                Draw = ListPO.Draw
            };
            IQueryable<V_PO2> query = null;
            query = _context.V_PO2.Where(x => x.Act_End_ASY == null).AsQueryable();
            query = query.OrderBy(x => x.Line).ThenBy(x => x.PO);//order by line asc, po w/ seq asc
            //query = query.OrderBy(x => x.PO);
            bool isFilterNull = string.IsNullOrEmpty(ListPO.SearchCriteria.Filter); // po
            bool isFilter2Null = string.IsNullOrEmpty(ListPO.SearchCriteria.Filter2);   //model
            bool isFilter3Null = string.IsNullOrEmpty(ListPO.SearchCriteria.Filter3);   //line

            if(!isFilterNull)
            {
                query = query.Where(x => x.MO_No.Contains(ListPO.SearchCriteria.Filter));
            }
            if(!isFilter2Null)
            {
                query = query.Where(x => x.Style_Name.Contains(ListPO.SearchCriteria.Filter2));
            }
            if(!isFilter3Null)
            {
                query = query.Where(x => x.Line.Contains(ListPO.SearchCriteria.Filter3));
            }
            
            var recordsTotal = query.Count();
            
            //no use, order default by line asc, po w/ seq asc
            //if (ListPO.Order.Count > 0)
            //{
            //    var colOrder = ListPO.Order[0];
            //    var colNameOrder = ListPO.Columns[colOrder.Column].Data;
            //    switch(colNameOrder)
            //    {
            //        case "qty":
            //            query = colOrder.Dir == "asc" ? query.OrderBy(x => x.Qty) : query.OrderByDescending(x => x.Qty);
            //            break;
            //    }
            //}
            var FinalArray = await query.ProjectTo<V_PO2DTO>(_configMapper).Skip(ListPO.Start).Take(ListPO.Length).ToArrayAsync();
            result.Data = FinalArray;
            result.RecordsTotal = recordsTotal;
            result.RecordsFiltered = recordsTotal;

            return result;
        }

        public async Task<bool> GenerateQR(RequestData data)
        {
            var PrepParam = data.Prep.ToList();
            var StiParam = data.Sti.ToList();
            Guid IdInsertedProcessStatus = Guid.Empty;

            if (StiParam.Count != 0)
            {
                int cnt = 1;
                for(var i = 0; i < StiParam.Count; i++)
                {
                    if (i==0)
                    {   //bikin qr baru
                        IdInsertedProcessStatus = GAddProcessStatus("STI");
                        if (await GAddRunningPO(StiParam[i], "STI", IdInsertedProcessStatus))
                        cnt++;
                    }
                    else
                    {
                        if(StiParam[i].Article == StiParam[i-1].Article || StiParam[i].Line == StiParam[i-1].Line) // line n artikel sama dengan sebelumnya
                        {
                            if(cnt>2)
                            {
                                if(StiParam[i].Qty < 50 && i == StiParam.Count-1) //sampe urutan terakhir dari array
                                {
                                    if (await GAddRunningPO(StiParam[i], "STI", IdInsertedProcessStatus)) //ga bikin
                                    cnt++;
                                }
                                else if (StiParam[i].Qty < 50 && (StiParam[i].Article != StiParam[i+1].Article || StiParam[i].Line != StiParam[i+1].Line)) //sampe urutan terakhir grup line & model
                                {
                                    if (await GAddRunningPO(StiParam[i], "STI", IdInsertedProcessStatus)) //ga bikin
                                    cnt++;
                                }
                                else //bikin qr baru
                                {
                                    IdInsertedProcessStatus = GAddProcessStatus("STI");
                                    if (await GAddRunningPO(StiParam[i], "STI", IdInsertedProcessStatus))
                                    cnt = 2;
                                }
                            }
                            else
                            {
                                if (await GAddRunningPO(StiParam[i], "STI", IdInsertedProcessStatus))
                                cnt++;
                            }
                        }
                        else
                        {
                            IdInsertedProcessStatus = GAddProcessStatus("STI");
                            if (await GAddRunningPO(StiParam[i], "STI", IdInsertedProcessStatus))
                            cnt = 2;
                        }
                    }
                }
            }

            if (PrepParam.Count != 0)
            {
                int cnt = 1;
                for(var i = 0; i < PrepParam.Count; i++)
                {
                    if (i==0)
                    {
                        IdInsertedProcessStatus = GAddProcessStatus("PREP");
                        if (await GAddRunningPO(PrepParam[i], "PREP", IdInsertedProcessStatus))
                        cnt++;
                    }
                    else
                    {
                        if(PrepParam[i].Article == PrepParam[i-1].Article || PrepParam[i].Line == PrepParam[i-1].Line) // line n artikel sama dengan sebelumnya
                        {
                            if(cnt>2)
                            {
                                if(PrepParam[i].Qty < 50 && i == PrepParam.Count-1) //sampe urutan terakhir dari array
                                {
                                    if (await GAddRunningPO(PrepParam[i], "PREP", IdInsertedProcessStatus))
                                    cnt++;
                                }
                                else if (PrepParam[i].Qty < 50 && (PrepParam[i].Article != PrepParam[i+1].Article || PrepParam[i].Line != PrepParam[i+1].Line)) //sampe urutan terakhir grup line & model
                                {
                                    if (await GAddRunningPO(PrepParam[i], "PREP", IdInsertedProcessStatus))
                                    cnt++;
                                }
                                else //bikin qr baru
                                {
                                    IdInsertedProcessStatus = GAddProcessStatus("PREP");
                                    if (await GAddRunningPO(PrepParam[i], "PREP", IdInsertedProcessStatus))
                                    cnt = 2;
                                }
                            }
                            else
                            {
                                if (await GAddRunningPO(PrepParam[i], "PREP", IdInsertedProcessStatus))
                                cnt++;
                            }
                        }
                        else
                        {
                            IdInsertedProcessStatus = GAddProcessStatus("PREP");
                            if (await GAddRunningPO(PrepParam[i], "PREP", IdInsertedProcessStatus))
                            cnt = 2;
                        }
                    }
                }
            }
            
            return true;
        }

        //g for generateqr func
        public Guid GAddProcessStatus(string _kind)
        {
            var _id = Guid.NewGuid();
            if(_kind == "STI")
            {
                var procStiStat = new ProcessStatus
                {
                    Id = _id,
                    Kind = _kind,
                    QRCode = "STI",
                    Status = "WAITING",
                    GenerateAt = DateTime.Now,
                    GenerateBy = "user login",
                    CreateAt = DateTime.Now
                };
                _context.ProcessStatus.Add(procStiStat);
            }
            else if (_kind == "PREP")
            {
                var procPrepStat = new ProcessStatusPreparation
                {
                    Id = _id,
                    Kind = _kind,
                    QRCode = "PREP",
                    Status = "WAITING",
                    GenerateAt = DateTime.Now,
                    GenerateBy = "user login",
                    CreateAt = DateTime.Now
                };
                _context.ProcessStatusPreparation.Add(procPrepStat);
            }
            
            _context.SaveChanges();
            return _id;
        }

        //g for generateqr func
        public async Task<bool> GAddRunningPO(V_PO2DTO data, string _kind, Guid IdProccStat)
        {
            //check if exist, no need to create, just update
            var dataSearch = _context.RunningPO.Where(x => x.Line == data.Line).Where(x => x.PO == data.PO).FirstOrDefault();
            if (dataSearch == null ) //create
            {
                if(_kind == "STI")
                {
                    var proccStiData = _context.ProcessStatus.Where(x => x.Id == IdProccStat).First();
                    proccStiData.QRCode = proccStiData.QRCode+";"+data.PO;
                    proccStiData.Cell = data.Line;
                    var newRunningPO = new RunningPO
                    {
                        Id = Guid.NewGuid(),
                        Factory = data.Factory,
                        Line = data.Line,
                        PO = data.PO,
                        Article = data.Article,
                        StiStatId = IdProccStat,
                        PrepStatId = null
                    };
                    _context.RunningPO.Add(newRunningPO);
                    _context.ProcessStatus.Update(proccStiData);
                    
                }
                else if (_kind == "PREP")
                {
                    var proccPrepData = _context.ProcessStatusPreparation.Where(x => x.Id == IdProccStat).First();
                    proccPrepData.QRCode = proccPrepData.QRCode+";"+data.PO;
                    proccPrepData.Cell = data.Line;
                    var newRunningPO = new RunningPO
                    {
                        Id = Guid.NewGuid(),
                        Factory = data.Factory,
                        Line = data.Line,
                        PO = data.PO,
                        Article = data.Article,
                        StiStatId = null,
                        PrepStatId = IdProccStat
                    };
                    _context.RunningPO.Add(newRunningPO);
                    _context.ProcessStatusPreparation.Update(proccPrepData);
                }
                
            }
            else //update
            {
                if(_kind == "STI")
                {
                    var proccStiData = _context.ProcessStatus.Where(x => x.Id == IdProccStat).First();
                    proccStiData.QRCode = proccStiData.QRCode+";"+data.PO;
                    proccStiData.Cell = data.Line;
                    dataSearch.StiStatId = IdProccStat;
                    _context.ProcessStatus.Update(proccStiData);
                }
                else if (_kind == "PREP")
                {
                    var proccPrepData = _context.ProcessStatusPreparation.Where(x => x.Id == IdProccStat).First();
                    proccPrepData.QRCode = proccPrepData.QRCode+";"+data.PO;
                    proccPrepData.Cell = data.Line;
                    dataSearch.PrepStatId = IdProccStat;
                    _context.ProcessStatusPreparation.Update(proccPrepData);
                }
                _context.RunningPO.Update(dataSearch);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DataTablesResponse<ProcessStat>> PreparationListSearch(DataTablesRequest ListPrep)
        {   //show Waiting status list
            //---------
            IQueryable<V_PO2> listPo = null;
            var prepWaitingStat = _context.ProcessStatusPreparation.Where(x => x.ScanAt == null).AsQueryable(); //cari yang sudah di scan
            listPo = _context.V_PO2.Where(x => x.PrepStatId != null).AsQueryable();
            var ListPo =  await listPo.ProjectTo<V_PO2DTO>(_configMapper).ToArrayAsync();
            List<ProcessStat> listStatus = new List<ProcessStat>();
            foreach (var lqr in prepWaitingStat)
            {
                var ajg = new ProcessStat();
                ajg.Id = lqr.Id.ToString();
                ajg.Kind = lqr.Kind;
                ajg.QRCode = lqr.QRCode;
                ajg.Status = lqr.Status;
                ajg.Cell = lqr.Cell;
                ajg.GenerateAt = lqr.GenerateAt;
                ajg.GenerateBy = lqr.GenerateBy;
                ajg.ScanAt = lqr.ScanAt;
                ajg.ScanBy = lqr.ScanBy;
                ajg.ScanDeliveryAt = lqr.ScanDeliveryAt;
                ajg.ScanDeliveryBy = lqr.ScanDeliveryBy;
                ajg.CreateAt = lqr.CreateAt;
                ajg.UpdateAt = lqr.UpdateAt;
                ajg.POlist = ListPo.Where(x => x.PrepStatId == lqr.Id.ToString().ToUpper()).ToArray();

                listStatus.Add(ajg);
            }
            //=======
            var result = new DataTablesResponse<ProcessStat>()
            {
                Draw = ListPrep.Draw
            };
            IQueryable<ProcessStat> query = listStatus.AsQueryable();
            query = query.OrderBy(x => x.GenerateAt);
            bool isFilterNull = string.IsNullOrEmpty(ListPrep.SearchCriteria.Filter); 

            if(!isFilterNull)
            {
                query = query.Where(x => x.QRCode.Contains(ListPrep.SearchCriteria.Filter));
            }
            
            var recordsTotal = query.Count();
            
            //no use, order default by line asc, po w/ seq asc
            if (ListPrep.Order.Count > 0)
            {
                var colOrder = ListPrep.Order[0];
                var colNameOrder = ListPrep.Columns[colOrder.Column].Data;
                switch(colNameOrder)
                {
                    case "cell":
                        query = colOrder.Dir == "asc" ? query.OrderBy(x => x.Cell) : query.OrderByDescending(x => x.Cell);
                        break;
                }
            }
            var FinalArray = query.Skip(ListPrep.Start).Take(ListPrep.Length).ToArray();
            result.Data = FinalArray;
            result.RecordsTotal = recordsTotal;
            result.RecordsFiltered = recordsTotal;

            return result;
        }

        public async Task<DataTablesResponse<ProcessStat>> StitchingListSearch(DataTablesRequest ListSti)
        {   //show Waiting status list
            //---------
            IQueryable<V_PO2> listPo = null;
            var stiWaitingStat = _context.ProcessStatus.Where(x => x.ScanAt == null).AsQueryable(); //cari yang sudah di scan
            listPo = _context.V_PO2.Where(x => x.StiStatId != null).AsQueryable();
            var ListPo =  await listPo.ProjectTo<V_PO2DTO>(_configMapper).ToArrayAsync();
            List<ProcessStat> listStatus = new List<ProcessStat>();
            foreach (var lqr in stiWaitingStat)
            {
                var ajg = new ProcessStat();
                ajg.Id = lqr.Id.ToString();
                ajg.Kind = lqr.Kind;
                ajg.QRCode = lqr.QRCode;
                ajg.Status = lqr.Status;
                ajg.Cell = lqr.Cell;
                ajg.GenerateAt = lqr.GenerateAt;
                ajg.GenerateBy = lqr.GenerateBy;
                ajg.ScanAt = lqr.ScanAt;
                ajg.ScanBy = lqr.ScanBy;
                ajg.ScanDeliveryAt = lqr.ScanDeliveryAt;
                ajg.ScanDeliveryBy = lqr.ScanDeliveryBy;
                ajg.CreateAt = lqr.CreateAt;
                ajg.UpdateAt = lqr.UpdateAt;
                ajg.POlist = ListPo.Where(x => x.StiStatId == lqr.Id.ToString().ToUpper()).ToArray();

                listStatus.Add(ajg);
            }
            //=======
            var result = new DataTablesResponse<ProcessStat>()
            {
                Draw = ListSti.Draw
            };
            IQueryable<ProcessStat> query = listStatus.AsQueryable();
            query = query.OrderBy(x => x.GenerateAt);
            bool isFilterNull = string.IsNullOrEmpty(ListSti.SearchCriteria.Filter); 

            if(!isFilterNull)
            {
                query = query.Where(x => x.Cell.Contains(ListSti.SearchCriteria.Filter));
            }
            
            var recordsTotal = query.Count();
            
            //no use, order default by line asc, po w/ seq asc
            if (ListSti.Order.Count > 0)
            {
                var colOrder = ListSti.Order[0];
                var colNameOrder = ListSti.Columns[colOrder.Column].Data;
                switch(colNameOrder)
                {
                    case "cell":
                        query = colOrder.Dir == "asc" ? query.OrderBy(x => x.Cell) : query.OrderByDescending(x => x.Cell);
                        break;
                }
            }
            var FinalArray = query.Skip(ListSti.Start).Take(ListSti.Length).ToArray();
            result.Data = FinalArray;
            result.RecordsTotal = recordsTotal;
            result.RecordsFiltered = recordsTotal;

            return result;
        }

        public async Task<bool> PrepQRDelete(ProcessStat prepQRdata)   
        {
            Guid? id = Guid.Empty;
            var listpo = prepQRdata.POlist;
            foreach (var data in listpo)
            {
                var runningPOs = _context.RunningPO.Where(x => x.Line == data.Line && x.PO == data.PO).SingleOrDefault();
                id = runningPOs.PrepStatId;
                runningPOs.PrepStatId = null;
                _context.RunningPO.Update(runningPOs);
                await _context.SaveChangesAsync();
            }
            var findprocessstaus = _context.ProcessStatusPreparation.Find(id, "PREP"); // cari ini sebelum updae ke null
            if (findprocessstaus != null)
            {
                _context.ProcessStatusPreparation.Remove(findprocessstaus);
                await _context.SaveChangesAsync();
            }
            
            return true;
        }

        public async Task<bool> StiQRDelete(ProcessStat stiQRdata)   
        {
            Guid? id = Guid.Empty;
            var listpo = stiQRdata.POlist;
            foreach (var data in listpo)
            {
                var runningPOs = _context.RunningPO.Where(x => x.Line == data.Line && x.PO == data.PO).SingleOrDefault();
                id = runningPOs.StiStatId;
                runningPOs.StiStatId = null;
                _context.RunningPO.Update(runningPOs);
                await _context.SaveChangesAsync();
            }
            var findprocessstaus = _context.ProcessStatus.Find(id, "STI"); // cari ini sebelum updae ke null
            if (findprocessstaus != null)
            {
                _context.ProcessStatus.Remove(findprocessstaus);
                await _context.SaveChangesAsync();
            }
            
            return true;
        }

    }
}