using System;
using System.Linq;
using CovidDataCollector.Managers;
using CovidDataCollector.Models;
using CovidDataCollector.Services;
using Microsoft.AspNetCore.Mvc;

namespace CovidDataCollector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UIDataController : ControllerBase
    {
        private readonly IPlotService _uiUtilService;
        private readonly ICovidStatManager _covidStatManager;

        public UIDataController(IPlotService uiUtilService, ICovidStatManager covidStatManager)
        {
            _uiUtilService = uiUtilService;
            _covidStatManager = covidStatManager;
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

        [HttpGet("GetStatModel")]
        public IActionResult GetFullTable()
        {
            var models = _uiUtilService.GetTableData();
            models.Reverse();

            var covidStatModel = models.First();

            var predictedCasesData = _covidStatManager.GetPredictionData("new_cases");
            var predictedDeathsData = _covidStatManager.GetPredictionData("new_deaths");

            return Ok(new CovidStatModel
            {
                new_cases = covidStatModel.new_cases,
                new_deaths = (double)covidStatModel.new_deaths,
                total_cases = covidStatModel.total_cases,
                total_deaths = (double)covidStatModel.total_deaths,
                cases_for_tomorrow = Math.Round(predictedCasesData.First()),
                deaths_for_tomorrow = Math.Round(predictedDeathsData.First())
            });
        }
    }
}
