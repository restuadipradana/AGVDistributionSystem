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
            var PrepParam = CheckedData.Prep.ToList();
            var StiPardam = CheckedData.Sti.ToList();
            //var PrepParam = DTParam.Value<JObject>("prep").ToObject<RequestData>();
            //var kjas = DTParam.SelectToken("prep").ToL ist();
            //foreach ( var assz in kjas){
            //    var ss = assz.ToList();
            //}
            //var lists = await _master.ProductDetailSearch(DTParam, ProductMainId);
            var res = await _mainService.GenerateQR(CheckedData);
            return Ok();
        }
    }
}