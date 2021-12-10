using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CovidDataCollector.Managers;
using CovidDataCollector.Models;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace CovidDataCollector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CovidDataController : ControllerBase
    {
        private readonly ICovidStatManager _covidStatManager;

        public CovidDataController(ICovidStatManager covidStatManager)
        {
            _covidStatManager = covidStatManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(string countryCode = "ukr")
        {
            //TODO: Map on "convenient" Dto
            var result = await _covidStatManager.GetCovidStatByCountryCode(countryCode);

            WriteToCsvLocalStorage(result.data);
            return Ok(result);
        }

        private static string GetRootPath()
        {
            //TODO: Set app it via app config
            return Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\..\\CsvStorage\\covidData.csv";
        }

        public void WriteToCsvLocalStorage(List<DailyCovidStatModel> covidStatModel)
        {
            using var sw = new StreamWriter(GetRootPath(), false, new UTF8Encoding(true));
            using var cw = new CsvWriter(sw, CultureInfo.CurrentCulture);

            cw.WriteHeader<DailyCovidStatModel>();
            cw.NextRecord();

            covidStatModel.ForEach(dailyStat =>
            {
                cw.WriteRecord(dailyStat);
                cw.NextRecord();
            });
        }
    }
}