using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CovidDataCollector.Managers;
using CovidDataCollector.Models;
using CovidDataCollector.Services;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace CovidDataCollector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UIDataController : ControllerBase
    {
        private readonly IPlotService _uiUtilService;

        public UIDataController(IPlotService uiUtilService)
        {
            _uiUtilService = uiUtilService;
        }

        [HttpGet("GetDeathPlot")]
        public IActionResult GetDeathPlot()
        {
            var path = _uiUtilService.BuildDeathsWithPredictionPlot();
            return PhysicalFile(path, "image/jpeg");
        }

        [HttpGet("GetCasesPlot")]
        public IActionResult GetCasesPlot()
        {
            var path = _uiUtilService.BuildNewCasesWithPredictionPlot();
            return PhysicalFile(path, "image/jpeg");
        }

        [HttpGet("GetFullTable")]
        public IActionResult GetFullTable()
        {
            return Ok(_uiUtilService.GetTableData());
        }
    }
}