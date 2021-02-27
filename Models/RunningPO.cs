using System;
using System.ComponentModel.DataAnnotations;

namespace AGVDistributionSystem.Models
{
    public class RunningPO
    {
        [Key]
        public Guid Id {get; set;}
        public Guid PrepStatId {get; set;}
        public Guid StiStatId {get; set;}

        [Required]
        public string PO {get; set;}

        [Required]
        public string Factory {get; set;}

        [Required]
        public string Line {get; set;}

        [Required]
        public string Article {get; set;}

    }
}