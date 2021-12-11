using CovidDataCollector.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CovidDataCollector.Extensions;
using CovidDataCollector.Properties;
using CovidDataCollector.Serializer;
using CovidDataCollector.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CovidStatManager> _logger;
        private static readonly HttpClient Client = new();

        public CovidStatManager(IDistributedCache distributedCache,
            GithubJsonCovidStatSource githubJsonCovidStatSource, ILogger<CovidStatManager> logger)
        {
            _distributedCache = distributedCache;
            _covidStatSource = githubJsonCovidStatSource;
            _logger = logger;
        }

        // Implementation of proxy pattern. If cache is not accessible once - disable it for some time
        public async Task<BaseCovidStatModel> GetCovidStatByCountryCode(string countryCode)
        {
            countryCode = countryCode.ToUpper();
            string recordKey = countryCode + DateAndTime.Now.ToString("yyyyMMMdd");

            var covidStat = await GetCovidStatByCountryCodeFromCache(recordKey) ??
                            await GetCovidStatByCountryCodeFromApi(countryCode, recordKey);

            try
            {
                await _distributedCache.SetRecordAsync(recordKey, covidStat);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Unsuccessful attempt to cache record in redis. Record: {recordKey}");
            }

            return covidStat;
        }

        public List<DailyCovidStatModel> GetRealData()
        {
            return XlsService.ReadCsv();
        }

        public List<double> GetPredictionData(string fileExtension)
        {
            return XlsService.ReadCsvDoubleCol(fileExtension);
        }

        private async Task<BaseCovidStatModel> GetCovidStatByCountryCodeFromCache(string recordKey)
        {
            BaseCovidStatModel covidStat = null;
            try
            {
                covidStat = await _distributedCache.GetRecordAsync<BaseCovidStatModel>(recordKey);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unsuccessful attempt to get the data from redis. Record: {recordKey}");
            }
            return covidStat;
        }

        private async Task<BaseCovidStatModel> GetCovidStatByCountryCodeFromApi(string countryCode, string recordKey)
        {
            var jsonData = await Client.GetStringAsync(_covidStatSource.Url);
            jsonData = CutOffRedundantData(countryCode, jsonData);

            return CovidStatSerializer.DeserializeJson(countryCode, jsonData);
        }

        private static string CutOffRedundantData(string countryCode, string jsonData)
            => "{" + $"\"{countryCode}" + jsonData.Split(countryCode)[1];
    }
}