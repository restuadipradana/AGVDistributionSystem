using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using AGVDistributionSystem._Services.Interfaces;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Helpers;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.Data;

namespace AGVDistributionSystem._Services.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public UserService(DataContext context, IMapper mapper, MapperConfiguration configMapper)
        {
            _context = context;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<List<UserHasLoggedDTO>> GetUser()
        {
            var userRole = _context.UserRole;
            var asdsd = userRole.GroupBy(x => x.Account).Select(a => a.Key).ToList();
            var usr = await _context.VW_UserAcc.Where(x => asdsd.Contains(x.account)).Select(x => new UserHasLoggedDTO 
            {
                Account = x.account,
                Name = x.vname
            }).ToListAsync();

            return usr;
        }

        public async Task<UserHasLoggedDTO> GetUserDetail(string account)
        {
            var userRoleList = _context.UserRole.Where(x => x.Account.Trim() == account);
            var user = _context.VW_UserAcc.Where(x => x.account.Trim() == account);
            var usr = await _context.VW_UserAcc.Where(x => x.account.Trim() == account).Select(x => new UserHasLoggedDTO 
            {
                Account = x.account,
                Name = x.vname,
                Role = userRoleList.Select(x => x.Role).ToList()
            }).FirstOrDefaultAsync();

            return usr;
        }

        public async Task<List<RolesDTO>> GetListRole()
        {
            var roleList = _context.Roles;
            return await roleList.ProjectTo<RolesDTO>(_configMapper).ToListAsync();
        }
        public async Task<List<RoleByUserDTO>> GetRoleByUser(string account)
        {
            var roleByUser = await _context.UserRole.Where(x => x.Account == account).Select(x => x.Role).ToListAsync();
            var role = await _context.Roles.Select(x => new RoleByUserDTO
            {
                Role= x.Role,
                RoleName = x.RoleName,
                Status = roleByUser.Contains(x.Role) == true ? true : false
            }).ToListAsync();
            return role;
        }
        public async Task<bool> EditUserRole(List<RoleByUserDTO> roles, string account, string createBy)
        {
            var userRole = _context.UserRole.Where(x => x.Account == account);
            _context.UserRole.RemoveRange(userRole);
            var newRole = roles.Select(x => new UserRoleDTO
                                                {
                                                    Account = account,
                                                    Role = x.Role,
                                                    CreateBy = createBy,
                                                    CreateAt = DateTime.Now
                                                }).ToList();
            var newUserRole = _mapper.Map<List<UserRole>>(newRole);
            _context.UserRole.AddRange(newUserRole);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CheckUserAvailable(string account)
        {
            var data = _context.VW_UserAcc.Where(x => x.account == account);
            if (await data.AnyAsync())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<string> GetNameUser(string account)
        {
            var data = await _context.VW_UserAcc.Where(x => x.account == account).Select(x => x.vname).SingleOrDefaultAsync();
            if (data != null)
            {
                return data;
            }
            else
            {
                return "something wrong!!";
            }
        }
    }
}