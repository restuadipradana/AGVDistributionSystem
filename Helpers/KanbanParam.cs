using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Models;

namespace AGVDistributionSystem.Helpers
{
    public class CellStatus //process status (deprecated)
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
    }

    public class ListCell
    {
        public ProcessStat[] Cell_1 {get; set;}
        public ProcessStat[] Cell_2 {get; set;}
        public ProcessStat[] Cell_3 {get; set;}
        public ProcessStat[] Cell_4 {get; set;}
        public ProcessStat[] Cell_5 {get; set;}
        public ProcessStat[] Cell_6 {get; set;}
        public ProcessStat[] Cell_7 {get; set;}
        public ProcessStat[] Cell_8 {get; set;}
        public ProcessStat[] Cell_9 {get; set;}
        public ProcessStat[] Cell_A {get; set;}
        public ProcessStat[] Cell_B {get; set;}
        public ProcessStat[] Cell_C {get; set;}
        public ProcessStat[] Cell_D {get; set;}
        public ProcessStat[] Cell_E {get; set;}
    }

    public class KanbanBuilding
    {
        public int BuildingNo {get; set;}
        public string BuildingName {get; set;}
        public int Ready {get; set;}
        public int Delivery {get; set;}
        public int Finished {get; set;}
        public int Prepared {get; set;}
    }
}