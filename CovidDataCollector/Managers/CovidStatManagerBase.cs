using CovidDataCollector.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidDataCollector.Managers
{
    public abstract class CovidStatManagerBase : ICovidStatManager
    {
        // TODO: fetch from appsettings url
        private const string GithubJsonCovidStatSource = "https://covid.ourworldindata.org/data/owid-covid-data.json";
        private static readonly HttpClient Client = new HttpClient();

        public async Task<BaseCovidStatModel> GetCovidStatByCountryCode(string countryCode)
        {
            countryCode = countryCode.ToUpper();
            var jsonData = await Client.GetStringAsync(GithubJsonCovidStatSource);
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
