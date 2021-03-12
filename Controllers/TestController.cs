using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AGVDistributionSystem._Services.Interfaces;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.DTO;

using AGVDistributionSystem.Helpers;

namespace AGVDistributionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet("getorg")]
        public async Task<IActionResult> GetAllOrg()
        {
            var data = await _testService.GetOrgAsync();
            return Ok(data);
        }

        [HttpGet("getuse")]
        public async Task<IActionResult> GetAllUser()
        {
            var data = await _testService.GetAllUser();
            return Ok(data);
        }

        [HttpGet("getuser1")]
        public async Task<IActionResult> GetSingleUser(string account)
        {
            var data = await _testService.GetUserAsync(account);
            return Ok(data);
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetOneStr()
        {
            var lists = await _testService.GetOneStr();
            return Ok(lists);
        }

        // test real case

        [HttpPost("po-list")]
        public async Task<IActionResult> ListPOSearch([FromBody]DataTablesRequest ListPOparam)
        {
            var lists = await _testService.POListSearch(ListPOparam);
            return Ok(lists);
        }
    }
}