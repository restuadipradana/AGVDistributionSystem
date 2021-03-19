using System.Threading.Tasks;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Helpers;
using System.Collections.Generic;


namespace AGVDistributionSystem._Services.Interfaces
{
    public interface IKanbanService
    {
        Task<List<ListCell>> GetKanbanCell(string building);
        Task<List<KanbanBuilding>> GetKanbanBuilding();
    }
}