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
            var q = Directory.GetCurrentDirectory() + "\\CsvStorage\\covidData.csv";
            //TODO: Map on "convenient" Dto
            var result = await _covidStatManager.GetCovidStatByCountryCode(countryCode);

            WriteNewCsvFile(q, result.data);
            return Ok(result);
        }

        public void WriteNewCsvFile(string path, List<DailyCovidStatModel> covidStatModel)
        {
            using var sw = new StreamWriter(path, false, new UTF8Encoding(true));
            using var cw = new CsvWriter(sw, CultureInfo.CurrentCulture);

            cw.WriteHeader<DailyCovidStatModel>();
            cw.NextRecord();
            foreach (var stat in covidStatModel)
            {
                cw.WriteRecord(stat);
                cw.NextRecord();
            }
        }
    }
}