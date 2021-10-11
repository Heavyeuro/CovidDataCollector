using CovidDataCollector.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
            //TODO: Map on "convinient" Dto
            var result = await _covidStatManager.GetCovidStatByCountryCode(countryCode);

            return Ok(result);
        }
    }
}
