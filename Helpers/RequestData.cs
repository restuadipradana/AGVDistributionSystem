using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Models;

namespace AGVDistributionSystem.Helpers
{
    public class RequestData //used for send checkbox generate qr
    {
        public V_PO2DTO[] Prep { get; set; }

        public V_PO2DTO[] Sti { get; set; }
    }

    public class RequestDataQR //used for send checkbox generate excel
    {
        public ProcessStat[] SelectedData { get; set; }
    }

    public class StatusView<T> //nouse
    {
        public V_PO2DTO[] ListPo { get; set; }
        public T[] ListQr { get; set; }
    }

//for show status at scam ready also kanban
    public class ProcessStat
    {
        public string Id {get; set;}
        public string Kind {get; set;}
        public string QRCode {get; set;}
        public string Status {get; set;}
        public string Cell {get; set;}
        public DateTime? GenerateAt {get; set;}
        public string GenerateBy {get; set;}
        public DateTime? ScanAt {get; set;}
        public string ScanBy {get; set;}
        public DateTime? ScanDeliveryAt {get; set;}
        public string ScanDeliveryBy {get; set;}
        public DateTime CreateAt {get; set;}
        public DateTime? UpdateAt {get; set;}
        public V_PO2DTO[] POlist {get; set;}
        public int TotQty {get; set;}
        public bool IsCheck //{get; set;}
        {
            get
            {
                return false;
            }
        }
    }
}