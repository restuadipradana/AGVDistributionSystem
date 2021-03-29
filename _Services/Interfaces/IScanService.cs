using System.Threading.Tasks;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Helpers;
using System.Collections.Generic;

namespace AGVDistributionSystem._Services.Interfaces
{
    public interface IScanService
    {
         Task<object> ScanReady(string processKind, string scanQr, string username);
         Task<object> ScanDelivery(string processKind, string scanQr, string username);
         Task<List<ProcessStat>> GetStatusSti(int flag);
         Task<List<ProcessStat>> GetStatusPrep(int flag);
    }
}