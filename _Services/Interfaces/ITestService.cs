using System.Threading.Tasks;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Helpers;
using System.Collections.Generic;

namespace AGVDistributionSystem._Services.Interfaces
{
    public interface ITestService
    {
         Task<object> GetAllUser();
         Task<List<VW_MES_OrgDTO>> GetOrgAsync();
         Task<VW_UserAccDTO> GetUserAsync(string account);
         Task<DataTablesResponse<V_PO2DTO>> POListSearch(DataTablesRequest ListPO);
         Task<string> GetOneStr();
         Task<List<ListCell>> GetKanban(string building);
    }
}