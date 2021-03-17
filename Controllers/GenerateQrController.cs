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
    public class GenerateQrController : ControllerBase
    {
        private readonly IMainService _mainService;
        public GenerateQrController(IMainService mainService)
        {
            _mainService = mainService;
        }

        [HttpPost("po-list")]
        public async Task<IActionResult> ListPOSearch([FromBody]DataTablesRequest ListPOparam)
        {
            var lists = await _mainService.POListSearch(ListPOparam);
            return Ok(lists);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GetSeletedData(JToken jtk)
        {        
            var CheckedData = jtk.Value<JObject>("dataTablesParam").ToObject<RequestData>();   
            var res = await _mainService.GenerateQR(CheckedData);
            return Ok();
        }

        [HttpPost("prep-list")]
        public async Task<IActionResult> PrepSearch([FromBody]DataTablesRequest ListPrep)
        {
            var lists = await _mainService.PreparationListSearch(ListPrep);
            return Ok(lists);
        }

        [HttpPost("sti-list")]
        public async Task<IActionResult> StiPOSearch([FromBody]DataTablesRequest ListSti)
        {
            var lists = await _mainService.StitchingListSearch(ListSti);
            return Ok(lists);
        }
    }
}