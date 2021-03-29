using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using AGVDistributionSystem._Services.Interfaces;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Helpers;
using Newtonsoft.Json.Linq;

namespace AGVDistributionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("user-list")]
        public async Task<IActionResult> GetAllUser()
        {
            var lists = await _userService.GetUser();
            var roles = await _userService.GetListRole();
            return Ok(new {lists, roles});
        }

        [HttpGet("user-detail")] //unused
        public async Task<IActionResult> GetUser(string account)
        {
            var lists = await _userService.GetUserDetail(account);
            return Ok(lists);
        }

        [HttpGet("roleuser/{account}")]
        public async Task<IActionResult> GetRoleByUser(string account)
        {
            var result = await _userService.GetRoleByUser(account);
            return Ok(result);
        }

        [HttpPost("roleuser/{account}")]
        public async Task<IActionResult> UpdateRoleUser(string account, List<RoleByUserDTO> roles)
        {
            var updateBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var createBy = updateBy.Trim();
            var result = await _userService.EditUserRole(roles, account, createBy);
            if (result)
            {
                return NoContent();
            }

            throw new Exception("Fail edit User");
        }

        [HttpGet("user-check/{account}")]
        public async Task<IActionResult> CheckUser(string account)
        {
            var result = await _userService.CheckUserAvailable(account);
            return Ok(result);
        }
    }
}