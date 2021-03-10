using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("scanready")]
        public async Task<IActionResult> ScanReady(string scanQr)
        {
            string[] rawdata = scanQr.Split(';');
            var data = await _scanService.ScanReady(rawdata[0], scanQr);
            return Ok(data);
        }

        [HttpGet("scandelivery")]
        public async Task<IActionResult> ScanDelivery(string scanQr)
        {
            string[] rawdata = scanQr.Split(';');
            var data = await _scanService.ScanDelivery(rawdata[0], scanQr);
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