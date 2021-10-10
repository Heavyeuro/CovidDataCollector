using CovidDataCollector.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using CovidDataCollector.Extensions;
using CovidDataCollector.Properties;
using CovidDataCollector.Serializer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualBasic;

namespace CovidDataCollector.Managers
{
    /// <summary>
    /// Generic implementation with reflection
    /// </summary>
    public class CovidStatManager : ICovidStatManager
    {
        private readonly IDistributedCache _distributedCache;
        private readonly GithubJsonCovidStatSource _covidStatSource;
        private static readonly HttpClient Client = new HttpClient();

        public CovidStatManager(IDistributedCache distributedCache, GithubJsonCovidStatSource githubJsonCovidStatSource)
        {
            _distributedCache = distributedCache;
            _covidStatSource = githubJsonCovidStatSource;
        }

        public async Task<BaseCovidStatModel> GetCovidStatByCountryCode(string countryCode)
        {
            countryCode = countryCode.ToUpper();

            BaseCovidStatModel covidStat = null;

            string recordKey = countryCode + DateAndTime.Now.ToString("yyyyMMMdd");
            try
            {
                covidStat = await _distributedCache.GetRecordAsync<BaseCovidStatModel>(recordKey);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (covidStat is null)
            {
                covidStat = await GetCovidStatByCountryCodeFromApi(countryCode);
                await _distributedCache.SetRecordAsync(recordKey, covidStat);
            }

            return covidStat;
        }

        private async Task<BaseCovidStatModel> GetCovidStatByCountryCodeFromApi(string countryCode)
        {
            var jsonData = await Client.GetStringAsync(_covidStatSource.Url);
            jsonData = CutOffRedundantData(countryCode, jsonData);

            return CovidStatSerializer.DeserializeJson(countryCode, jsonData);
        }

        private static string CutOffRedundantData(string countryCode, string jsonData)
            => "{" + $"\"{countryCode}" + jsonData.Split(countryCode)[1];
    }
}