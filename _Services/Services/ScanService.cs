using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
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
    public class ScanService : IScanService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ScanService(DataContext context, IMapper mapper, MapperConfiguration configMapper)
        {
            _context = context;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<object> ScanReady(string processKind, string scanQr)
        {
            if (processKind == "STI")
            {
                var qrSti = await _context.ProcessStatus.Where(x => x.QRCode == scanQr).FirstOrDefaultAsync();
                if(qrSti != null)
                {
                    if(qrSti.ScanAt != null)
                    {
                        return "this QR already scanned";
                    }
                    qrSti.ScanAt = DateTime.Now;
                    qrSti.ScanBy = "user admin";
                    qrSti.Status = "READY";
                    qrSti.UpdateAt = DateTime.Now;
                    _context.ProcessStatus.Update(qrSti);
                    await _context.SaveChangesAsync();
                    return qrSti;
                }
                else
                {
                    return "no valid data";
                }
            }
            else if (processKind == "PREP")
            {
                var qrPrep = await _context.ProcessStatusPreparation.Where(x => x.QRCode == scanQr).FirstOrDefaultAsync();
                if(qrPrep != null)
                {
                    if(qrPrep.ScanAt != null)
                    {
                        return "this QR already scanned";
                    }
                    qrPrep.ScanAt = DateTime.Now;
                    qrPrep.ScanBy = "user admin";
                    qrPrep.Status = "READY";
                    qrPrep.UpdateAt = DateTime.Now;
                    _context.ProcessStatusPreparation.Update(qrPrep);
                    await _context.SaveChangesAsync();
                    return qrPrep;
                }
                else
                {
                    return "no valid data";
                }
            }
            else
            {
                return "wrong qr code provided";
            }
        }

        public async Task<object> ScanDelivery(string processKind, string scanQr)
        {
            if (processKind == "STI")
            {
                var qrSti = await _context.ProcessStatus.Where(x => x.QRCode == scanQr && x.ScanAt != null).FirstOrDefaultAsync();
                if(qrSti != null)
                {
                    if(qrSti.ScanDeliveryAt != null)
                    {
                        return "this QR already scanned";
                    }
                    qrSti.ScanDeliveryAt = DateTime.Now;
                    qrSti.ScanDeliveryBy = "user agv";
                    qrSti.Status = "DELIVERY";
                    qrSti.UpdateAt = DateTime.Now;
                    _context.ProcessStatus.Update(qrSti);
                    await _context.SaveChangesAsync();
                    return qrSti;
                }
                else
                {
                    return "not scan ready yet";
                }
            }
            else if (processKind == "PREP")
            {
                var qrPrep = await _context.ProcessStatusPreparation.Where(x => x.QRCode == scanQr && x.ScanAt != null).FirstOrDefaultAsync();
                if(qrPrep != null)
                {
                    if(qrPrep.ScanDeliveryAt != null)
                    {
                        return "this QR already scanned";
                    }
                    qrPrep.ScanDeliveryAt = DateTime.Now;
                    qrPrep.ScanDeliveryBy = "user agv";
                    qrPrep.Status = "DELIVERY";
                    qrPrep.UpdateAt = DateTime.Now;
                    _context.ProcessStatusPreparation.Update(qrPrep);
                    await _context.SaveChangesAsync();
                    return qrPrep;
                }
                else
                {
                    return "not scan ready yet";
                }
            }
            else
            {
                return "wrong qr code provided";
            }
        }

        public async Task<List<ProcessStat>> GetStatusSti(int flag)
        {
            //flag 1 = today scanned view (scan ready menu), 2 = all scanned view (status menu)
            //percobaan pertama buat status scan ready
            //Task<StatusView<ProcessStatusDTO>>
            IQueryable<V_PO2> listPo = null;
            var stiScannedQr = _context.ProcessStatus.Where(x => x.ScanAt >= DateTime.Now.Date).OrderByDescending(o => o.ScanAt).AsQueryable(); //cari yang sudah di scan
            listPo = _context.V_PO2.Where(x => x.StiStatId != null).AsQueryable();
            var stiScannedPo = (from a in listPo
                                join b in stiScannedQr
                                on a.StiStatId equals b.Id
                                orderby b.ScanAt descending
                                select a).AsQueryable();

            var result = new StatusView<ProcessStatusDTO>()
            {
                ListQr = await stiScannedQr.ProjectTo<ProcessStatusDTO>(_configMapper).ToArrayAsync(),
                ListPo =  await stiScannedPo.ProjectTo<V_PO2DTO>(_configMapper).ToArrayAsync()
            };
            

            //percobaan ke2
            // Task<List<ProcessStat>>
            var polis = await listPo.ProjectTo<V_PO2DTO>(_configMapper).ToArrayAsync();
            
            List<ProcessStat> listStatus = new List<ProcessStat>();
            foreach (var lqr in stiScannedQr)
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
                ajg.POlist = result.ListPo.Where(x => x.StiStatId == lqr.Id.ToString().ToUpper()).ToArray();

                listStatus.Add(ajg);
            }
            return listStatus;
        }

        public async Task<List<ProcessStat>> GetStatusPrep(int flag)
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
            return listStatus;
        }
    }
}