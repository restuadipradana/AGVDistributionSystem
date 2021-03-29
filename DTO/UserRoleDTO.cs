using System;

namespace AGVDistributionSystem.DTO
{
    public class UserRoleDTO
    {
        public string Account {get; set;}
        public string Role {get; set;}
        public string CreateBy {get; set;}
        public DateTime CreateAt {get; set;}
    }
}