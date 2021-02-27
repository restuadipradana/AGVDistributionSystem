using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGVDistributionSystem.Models
{
    public class VW_UserAcc
    {
        public int id {get; set;}
        public string manuf {get; set;}
        public string account {get; set;}
        public string empno {get; set;}
        public string vname {get; set;}
        public string passw {get; set;}
        public string sex {get; set;}
        public string dept {get; set;}
        public string email {get; set;}
        public string token {get; set;}
        public string image {get; set;}

        [Column(TypeName = "date")]
        public DateTime indat {get; set;}
        public string intime {get; set;}

        [Column(TypeName = "date")]
        public DateTime? updat {get; set;}
        public string uptime {get; set;}

    }
}