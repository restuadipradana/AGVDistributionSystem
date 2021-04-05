using System;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
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

        [HttpPost("exportExcel")]
        public async Task<IActionResult> ExportExcel(JToken generatedData) {
            var CheckedData = generatedData.Value<JObject>("dataParam").ToObject<RequestDataQR>(); 
            var selectedQR = CheckedData.SelectedData.ToList();
            var stream = new MemoryStream();  
  
            using (var package = new ExcelPackage(stream))  
            {  
                var workSheet = package.Workbook.Worksheets.Add("1"); //harus angka 1 anjay

                workSheet.Cells["A1"].Value = "QRCode";
                workSheet.Cells["B1"].Value = "Kind";
                workSheet.Cells["C1"].Value = "Cell";
                workSheet.Cells["D1"].Value = "Art"; 
                workSheet.Cells["E1"].Value = "PO1"; 
                workSheet.Cells["F1"].Value = "PO2"; 
                workSheet.Cells["G1"].Value = "PO3"; 
                workSheet.Cells["H1"].Value = "Qty"; 
                workSheet.Cells["I1"].Value = "Date";
                int row = 2;
                foreach (var data in selectedQR) 
                {
                    workSheet.Cells["A" + row].Value = data.QRCode;
                    workSheet.Cells["B" + row].Value = data.Kind == "STI" ? "STITCHING" : "PREPARATION";
                    workSheet.Cells["C" + row].Value = data.Cell;
                    workSheet.Cells["D" + row].Value = data.POlist[0].Article;
                    workSheet.Cells["E" + row].Value = data.POlist[0].PO;
                    workSheet.Cells["F" + row].Value = data.POlist.Count() > 1 ? data.POlist[1].PO : " ";
                    workSheet.Cells["G" + row].Value = data.POlist.Count() > 2 ? data.POlist[2].PO : " ";
                    workSheet.Cells["H" + row].Value = data.TotQty;
                    workSheet.Cells["I" + row].Value = data.GenerateAt.Value.ToString("MM/dd/yyyy");
                    row++;
                }


                package.Save();  
            }  
            stream.Position = 0;  
            string excelName = $"ExcelQR-{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";  
        
            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/xlsx" , excelName);  

        }
    }
}