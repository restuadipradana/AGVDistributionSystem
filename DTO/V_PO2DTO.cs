using System;
using System.ComponentModel.DataAnnotations;

namespace AGVDistributionSystem.DTO
{
    public class V_PO2DTO
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
        public DateTime Comfirm_Date {get; set;}
        public DateTime CRD {get; set;}
        public string PrepStatId {get; set;}
        public string StiStatId {get; set;}
        public bool IsPrepCheck //{get; set;}
        {
            get
            {
                if(this.PrepStatId == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool IsStiCheck // {get; set;}
        {
            get
            {
                if(this.StiStatId == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public string PrepStat { get; set; }
        public string StiStat { get; set; }
    }
}