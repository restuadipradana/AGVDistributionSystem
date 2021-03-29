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
        Task<bool> GenerateQR(RequestData data, string username);
        Task<DataTablesResponse<ProcessStat>> PreparationListSearch(DataTablesRequest ListPrep);
        Task<DataTablesResponse<ProcessStat>> StitchingListSearch(DataTablesRequest ListSti);
        Task<bool> PrepQRDelete(ProcessStat prepQRdata);
        Task<bool> StiQRDelete(ProcessStat stiQRdata);
    }
}