using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//for stitching
namespace AGVDistributionSystem.Models
{
    public class ProcessStatus
    {
        [Key]
        [Column(Order = 0)]
        public Guid Id {get; set;}

        [Key]
        [Column(Order = 1)]
        public string Kind {get; set;}

        [Required]
        public string QRCode {get; set;}

        [Required]
        public string Status {get; set;}
        public DateTime GenerateAt {get; set;}
        public string GenerateBy {get; set;}
        public DateTime? ScanAt {get; set;}
        public string ScanBy {get; set;}
        public DateTime? ScanDeliveryAt {get; set;}
        public string ScanDeliveryBy {get; set;}

        [Required]
        public DateTime CreateAt {get; set;}
        public DateTime? UpdateAt {get; set;}
        public string Cell {get; set;}

    }
}