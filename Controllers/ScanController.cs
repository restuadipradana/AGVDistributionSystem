using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AGVDistributionSystem._Services.Interfaces;
using AGVDistributionSystem.Models;
using AGVDistributionSystem.DTO;
using AGVDistributionSystem.Helpers;
using Newtonsoft.Json.Linq;



namespace AGVDistributionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScanController : ControllerBase
    {
        private readonly IScanService _scanService;
        public ScanController(IScanService scanService)
        {
            _scanService = scanService;
        }

        private string GetUserClaim() {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Authorize]
        [HttpGet("scanready")]
        public async Task<IActionResult> ScanReady(string scanQr)
        {
            var username = GetUserClaim();
            string[] rawdata = scanQr.Split(';');
            var data = await _scanService.ScanReady(rawdata[0], scanQr, username);
            return Ok(data);
        }

        [Authorize]
        [HttpGet("scandelivery")]
        public async Task<IActionResult> ScanDelivery(string scanQr)
        {
            var username = GetUserClaim();
            string[] rawdata = scanQr.Split(';');
            var data = await _scanService.ScanDelivery(rawdata[0], scanQr, username);
            return Ok(data);
        }

        [HttpGet("getstistat-today")]
        public async Task<IActionResult> StiStatusListToday()
        {
            var lists = await _scanService.GetStatusSti(1);
            return Ok(lists);
        }

        [HttpGet("getprepstat-today")]
        public async Task<IActionResult> PrepStatusListToday()
        {
            var lists = await _scanService.GetStatusPrep(1);
            return Ok(lists);
        }
    }
}