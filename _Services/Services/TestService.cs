using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using AGVDistributionSystem._Services.Interfaces;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Helpers;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.Data;

namespace AGVDistributionSystem._Services.Services
{
    public class TestService : ITestService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public TestService(DataContext context, IMapper mapper, MapperConfiguration configMapper)
        {
            _context = context;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<object> GetAllUser()
        {
            return await _context.VW_UserAcc.Where(x => x.manuf == "U").ToListAsync();
        }

        public async Task<List<VW_MES_OrgDTO>> GetOrgAsync()
        {
            var data = await _context.VW_MES_Org.ToListAsync();
            var org = _mapper.Map<List<VW_MES_OrgDTO>>(data);
            return org;
        }

        public async Task<VW_UserAccDTO> GetUserAsync(string account)
        {
            var user = await _context.VW_UserAcc.Where(x => x.account.Trim() == account).FirstOrDefaultAsync();
            var data = _mapper.Map<VW_UserAccDTO>(user);
            return data;
        }

        //real test svc

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

        //percobaan lavgi
        public async Task<List<ProcessStat>> GetStatusPrepForKanban(int building)
        {
            //flag 1 = today scanned view (scan ready menu), 2 = all scanned view (status menu)
            IQueryable<V_PO2> listPo = null;
            var prepScannedQr = _context.ProcessStatusPreparation.Where(x => x.ScanAt >= DateTime.Now.Date).OrderByDescending(o => o.ScanAt).AsQueryable(); //cari yang sudah di scan
            listPo = _context.V_PO2.Where(x => x.PrepStatId != null).AsQueryable();
            var ListPo =  await listPo.ProjectTo<V_PO2DTO>(_configMapper).ToArrayAsync();
            
            List<ProcessStat> listStatus = new List<ProcessStat>();
            foreach (var lqr in prepScannedQr)
            {
                var ajg = new ProcessStat();
                ajg.Id = lqr.Id.ToString();
                ajg.Kind = lqr.Kind;
                ajg.QRCode = lqr.QRCode;
                ajg.Status = lqr.Status;
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
            return listStatus;
        }

        public async Task<string> GetOneStr()
        {
            DateTime currentTime = DateTime.Now;
            DateTime x30MinsLater = currentTime.AddMinutes(10);
            Console.WriteLine(string.Format("{0} {1}", currentTime, x30MinsLater));
            var dateres = string.Format("{0} {1}", currentTime, x30MinsLater);

            return dateres;
        }

    }
}