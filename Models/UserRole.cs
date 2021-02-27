using System;
using System.ComponentModel.DataAnnotations;

namespace AGVDistributionSystem.Models
{
    public class UserRole
    {
        [Required]
        public Guid Id {get; set;}

        [Required]
        public string Account {get; set;}

        [Required]
        public Guid RoleId {get; set;}

        [Required]
        public DateTime CreateAt {get; set;}

    }
}