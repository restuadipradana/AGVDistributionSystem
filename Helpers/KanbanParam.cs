using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Models;

namespace AGVDistributionSystem.Helpers
{
    public class CellStatus //process status
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
        public CellStatus[] Cell_1 {get; set;}
        public CellStatus[] Cell_2 {get; set;}
        public CellStatus[] Cell_3 {get; set;}
        public CellStatus[] Cell_4 {get; set;}
        public CellStatus[] Cell_5 {get; set;}
        public CellStatus[] Cell_6 {get; set;}
        public CellStatus[] Cell_7 {get; set;}
        public CellStatus[] Cell_8 {get; set;}
        public CellStatus[] Cell_9 {get; set;}
        public CellStatus[] Cell_A {get; set;}
        public CellStatus[] Cell_B {get; set;}
        public CellStatus[] Cell_C {get; set;}
        public CellStatus[] Cell_D {get; set;}
        public CellStatus[] Cell_E {get; set;}
    }

    public class KanbanData
    {
        public ListCell[] Preparation {get; set;}
        public ListCell[] Stitching {get; set;}
    }
}