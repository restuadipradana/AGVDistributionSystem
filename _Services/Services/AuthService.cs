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
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public AuthService(DataContext context, IMapper mapper, MapperConfiguration configMapper)
        {
            _context = context;
            _mapper = mapper;
            _configMapper = configMapper;
        }
                                                    //username = account
        public async Task<UserHasLoggedDTO> GetUser(string username, string password)
        {
            //var user = _repoUsers.FindSingle(x => x.account.Trim() == username.Trim() && x.is_active == true);
            var userRole = _context.UserRole.Where(x => x.Account.Trim() == username.Trim());
            var userAutho = await _context.VW_UserAcc.Where(x => x.account == username).FirstOrDefaultAsync();

            if (userRole.FirstOrDefault() == null)
            {
                return null;
            }
            if (userAutho.passw.Trim() != password.Trim())
            {
                return null;
            }
            var role = _context.Roles;
            var roleName = await userRole.Join(role, x => x.Role, y => y.Role, (x, y) => new RoleDTO { Name = x.Role, Position = y.RoleIndex }).ToListAsync();

            var result = new UserHasLoggedDTO
            {
                Account = userAutho.account,
                Name = userAutho.vname,
                Role = roleName.OrderBy(x => x.Position).Select(x => x.Name).ToList()
            };

            return result;
        }

    }
}