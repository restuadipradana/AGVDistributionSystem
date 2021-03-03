using Newtonsoft.Json;
using System.Collections.Generic;
using AGVDistributionSystem.DTO;

namespace AGVDistributionSystem.Helpers
{
    public class RequestData
    {
        public V_PO2DTO[] Prep { get; set; }

        public V_PO2DTO[] Sti { get; set; }
    }
}