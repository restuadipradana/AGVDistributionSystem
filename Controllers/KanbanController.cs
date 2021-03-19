using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AGVDistributionSystem._Services.Interfaces;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.DTO;

using AGVDistributionSystem.Helpers;

namespace AGVDistributionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KanbanController : ControllerBase
    {
        private readonly IKanbanService _kanbanService;
        public KanbanController(IKanbanService kanbanService)
        {
            _kanbanService = kanbanService;
        }

        [HttpGet("cell-kanban")]
        public async Task<IActionResult> GetCellKanban(string building)
        {
            var lists = await _kanbanService.GetKanbanCell(building);
            return Ok(lists);

        }

        [HttpGet("building-kanban")]
        public async Task<IActionResult> GetCBuildingKanban()
        {
            var lists = await _kanbanService.GetKanbanBuilding();
            return Ok(lists);

        }
    }
}