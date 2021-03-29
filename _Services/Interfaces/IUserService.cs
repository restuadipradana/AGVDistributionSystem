using System.Threading.Tasks;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Helpers;
using System.Collections.Generic;

namespace AGVDistributionSystem._Services.Interfaces
{
    public interface IUserService
    {
         Task<List<UserHasLoggedDTO>> GetUser();
         Task<UserHasLoggedDTO> GetUserDetail(string account);
         Task<List<RolesDTO>> GetListRole();
         Task<List<RoleByUserDTO>> GetRoleByUser(string account);
         Task<bool> EditUserRole(List<RoleByUserDTO> roles, string account, string createBy);
         Task<bool> CheckUserAvailable(string account);
    }
}