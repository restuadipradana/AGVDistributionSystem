using System;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    
    public class GenerateQrController : ControllerBase
    {
        private readonly IMainService _mainService;
        public GenerateQrController(IMainService mainService)
        {
            _mainService = mainService;
        }

        private string GetUserClaim() {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [HttpPost("po-list")]
        public async Task<IActionResult> ListPOSearch([FromBody]DataTablesRequest ListPOparam)
        {
            var lists = await _mainService.POListSearch(ListPOparam);
            return Ok(lists);
        }

        [Authorize]
        [HttpPost("generate")]
        public async Task<IActionResult> GetSeletedData(JToken jtk)
        {        
            var username = GetUserClaim();
            var CheckedData = jtk.Value<JObject>("dataTablesParam").ToObject<RequestData>();   
            var res = await _mainService.GenerateQR(CheckedData, username);
            return Ok();
        }

        [HttpPost("prep-list")]
        public async Task<IActionResult> PrepSearch([FromBody]DataTablesRequest ListPrep)
        {
            var lists = await _mainService.PreparationListSearch(ListPrep);
            return Ok(lists);
        }

        [HttpPost("sti-list")]
        public async Task<IActionResult> StiSearch([FromBody]DataTablesRequest ListSti)
        {
            var lists = await _mainService.StitchingListSearch(ListSti);
            return Ok(lists);
        }

        [HttpPost("prep-delete")]
        public async Task<IActionResult> PrepDelete(ProcessStat prepQRdata)
        {

            if (await _mainService.PrepQRDelete(prepQRdata))
            {
                return NoContent();
            }
            
            throw new Exception("Deleting failed on save");
        }

        [HttpPost("sti-delete")]
        public async Task<IActionResult> StiDelete(ProcessStat stiQRdata)
        {

            if (await _mainService.StiQRDelete(stiQRdata))
            {
                return NoContent();
            }
            
            throw new Exception("Deleting failed on save");
        }
    }
}