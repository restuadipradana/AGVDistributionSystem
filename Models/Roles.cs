using System;
using System.ComponentModel.DataAnnotations;

namespace AGVDistributionSystem.Models
{
    public class Roles
    {
        [Key]
        public string Role {get; set;}

        [Required]
        public string RoleName {get; set;}
        public string RoleDesc {get; set;}
        public int RoleIndex {get; set;}
        public DateTime CreateAt {get; set;}

    }
}