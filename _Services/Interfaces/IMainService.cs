using System.Threading.Tasks;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Helpers;
using System.Collections.Generic;

namespace AGVDistributionSystem._Services.Interfaces
{
    public interface IMainService
    {
        Task<DataTablesResponse<V_PO2DTO>> POListSearch(DataTablesRequest ListPO);
        Task<bool> GenerateQR(RequestData data);
    }
}