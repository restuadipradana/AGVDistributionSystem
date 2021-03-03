using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGVDistributionSystem.Models
{
    public class V_PO2
    {
        public string Factory {get; set;}
        public string Line {get; set;}
        public string MO_No {get; set;}
        public string MO_Seq {get; set;}
        public string PO {get; set;}
        public string Style_No {get; set;}
        public string Style_Name {get; set;}
        public string Article {get; set;}
        public int Qty {get; set;}
        public DateTime? Plan_Start_ASY {get; set;}
        public DateTime? Act_End_ASY {get; set;}

        //[Column(TypeName = "date")]
        //public DateTime? Comfirm_Date {get; set;}

        //[Column(TypeName = "date")]
        //public DateTime? CRD {get; set;}
        public Guid PrepStatId {get; set;}
        public Guid StiStatId {get; set;}
        public string PrepStat {get; set;}
        public string StiStat {get; set;}

    }
}