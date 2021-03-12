using System;

namespace AGVDistributionSystem.DTO
{
    public class ProcessStatusPreparationDTO
    {
        public string Id {get; set;}
        public string Kind {get; set;}
        public string QRCode {get; set;}
        public string Status {get; set;}
        public DateTime GenerateAt {get; set;}
        public string GenerateBy {get; set;}
        public DateTime ScanAt {get; set;}
        public string ScanBy {get; set;}
        public DateTime ScanDeliveryAt {get; set;}
        public string ScanDeliveryBy {get; set;}
        public DateTime CreateAt {get; set;}
        public DateTime UpdateAt {get; set;}
        public string Cell {get; set;}
    }
}