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
    public class KanbanService : IKanbanService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public KanbanService(DataContext context, IMapper mapper, MapperConfiguration configMapper)
        {
            _context = context;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<List<ListCell>> GetKanbanCell(string building)
        {
            //flag 1 = today scanned view (scan ready menu), 2 = all scanned view (status menu)
            
            var preparationQR = _context.ProcessStatusPreparation.Where(x => x.Cell.StartsWith(building))
                                .Where(x => x.ScanAt != null).Where(x => DateTime.Now < x.ScanDeliveryAt.Value.AddMinutes(10) || x.ScanDeliveryAt == null)
                                .OrderBy(o => o.ScanAt);

            var stitchingQR = _context.ProcessStatus.Where(x => x.Cell.StartsWith(building))
                                .Where(x => x.ScanAt != null).Where(x => DateTime.Now < x.ScanDeliveryAt.Value.AddMinutes(10) || x.ScanDeliveryAt == null)
                                .OrderBy(o => o.ScanAt);
            
            List<CellStatus> listStatus = new List<CellStatus>();
            foreach (var lqr in preparationQR)
            {
                var poprep = _context.V_PO2.Where(x => x.PrepStatId == lqr.Id).AsQueryable();
                var poprepdto = await poprep.ProjectTo<V_PO2DTO>(_configMapper).ToArrayAsync();
                var ajg = new CellStatus();
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
                ajg.POlist = poprepdto;

                listStatus.Add(ajg);
            }

            foreach (var lqr in stitchingQR)
            {
                var posti = _context.V_PO2.Where(x => x.StiStatId == lqr.Id).AsQueryable();
                var postidto = await posti.ProjectTo<V_PO2DTO>(_configMapper).ToArrayAsync();
                var ajg = new CellStatus();
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
                ajg.POlist = postidto;

                listStatus.Add(ajg);
            }

            List<ListCell> listCell = new List<ListCell>();
            var newListCell = new ListCell();
            newListCell.Cell_1 = listStatus.Where(x => x.Cell.StartsWith(building+"1")).ToArray();
            newListCell.Cell_2 = listStatus.Where(x => x.Cell.StartsWith(building+"2")).ToArray();
            newListCell.Cell_3 = listStatus.Where(x => x.Cell.StartsWith(building+"3")).ToArray();
            newListCell.Cell_4 = listStatus.Where(x => x.Cell.StartsWith(building+"4")).ToArray();
            newListCell.Cell_5 = listStatus.Where(x => x.Cell.StartsWith(building+"5")).ToArray();
            newListCell.Cell_6 = listStatus.Where(x => x.Cell.StartsWith(building+"6")).ToArray();
            newListCell.Cell_7 = listStatus.Where(x => x.Cell.StartsWith(building+"7")).ToArray();
            newListCell.Cell_8 = listStatus.Where(x => x.Cell.StartsWith(building+"8")).ToArray();
            newListCell.Cell_9 = listStatus.Where(x => x.Cell.StartsWith(building+"9")).ToArray();
            newListCell.Cell_A = listStatus.Where(x => x.Cell.StartsWith(building+"A")).ToArray();
            newListCell.Cell_B = listStatus.Where(x => x.Cell.StartsWith(building+"B")).ToArray();
            newListCell.Cell_C = listStatus.Where(x => x.Cell.StartsWith(building+"C")).ToArray();
            newListCell.Cell_D = listStatus.Where(x => x.Cell.StartsWith(building+"D")).ToArray();
            newListCell.Cell_E = listStatus.Where(x => x.Cell.StartsWith(building+"E")).ToArray();

            listCell.Add(newListCell);


            return listCell;
        }
    }
}