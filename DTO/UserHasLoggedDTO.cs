using System.Collections.Generic;

namespace AGVDistributionSystem.DTO
{
    public class UserHasLoggedDTO
    {
        public string Account {get; set;}
        public string Name {get; set;}
        public List<string> Role {get; set;}
    }
}