using System;
using System.ComponentModel.DataAnnotations;

namespace AGVDistributionSystem.Models
{
    public class VW_MES_Line
    {
        public string Factory_ID {get; set;}
        public string Line_ID {get; set;}
        public string Line_Desc {get; set;}
        public string Line_DescZW {get; set;}
        public string Line_DescVN {get; set;}
        public string Line_Sname {get; set;}
    }
}