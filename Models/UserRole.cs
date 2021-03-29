using System;
using System.ComponentModel.DataAnnotations;

namespace AGVDistributionSystem.Models
{
    public class UserRole
    {
        [Key]
        public string Account {get; set;}

        [Key]
        public string Role {get; set;}

        public string CreateBy {get; set;}

        [Required]
        public DateTime CreateAt {get; set;}

    }
}