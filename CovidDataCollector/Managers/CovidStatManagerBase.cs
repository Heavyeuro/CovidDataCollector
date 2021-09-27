using CovidDataCollector.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidDataCollector.Managers
{
    public abstract class CovidStatManagerBase : ICovidStatManager
    {
        private const string GithubJsonCovidStatSource = "https://covid.ourworldindata.org/data/owid-covid-data.json";
        private static readonly HttpClient client = new HttpClient();

        public async Task<BaseCovidStatModel> GetCovidStatByCountryCode(string countryCode)
        {   //TODO: add caching
            countryCode = countryCode.ToUpper();
            var jsonData = await client.GetStringAsync(GithubJsonCovidStatSource);
            jsonData = CutOffRedundantData(countryCode, jsonData);

            return DeserializeJson(countryCode, jsonData);
        }

        public abstract BaseCovidStatModel DeserializeJson(string countryCode, string jsonData);

        private string CutOffRedundantData(string countryCode, string jsonData)
        {
            return "{" + $"\"{countryCode}" + jsonData.Split(countryCode)[1];
        }
    }
}
