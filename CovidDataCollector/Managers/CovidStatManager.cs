using CovidDataCollector.Converters;
using CovidDataCollector.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using CovidDataCollector.Extensions;
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
        private const string GithubJsonCovidStatSource = "https://covid.ourworldindata.org/data/owid-covid-data.json";
        private static readonly HttpClient Client = new HttpClient();
        private readonly string _modelsNamespace;

        public CovidStatManager(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _modelsNamespace = typeof(BaseCovidStatModel).Namespace;
        }

        public async Task<BaseCovidStatModel> GetCovidStatByCountryCode(string countryCode)
        {
            countryCode = countryCode.ToUpper();

            string recordKey = countryCode + DateAndTime.Now.ToString("yyyyMMMdd");
            var covidStat = await _distributedCache.GetRecordAsync<BaseCovidStatModel>(recordKey);

            if (covidStat is null)
            {
                covidStat = await GetCovidStatByCountryCodeFromApi(countryCode);
                await _distributedCache.SetRecordAsync(recordKey, covidStat);
            }

            return covidStat;
        }

        private async Task<BaseCovidStatModel> GetCovidStatByCountryCodeFromApi(string countryCode)
        {
            var jsonData = await Client.GetStringAsync(GithubJsonCovidStatSource);
            jsonData = CutOffRedundantData(countryCode, jsonData);

            return DeserializeJson(countryCode, jsonData);
        }

        private BaseCovidStatModel DeserializeJson(string countryCode, string jsonData)
        {
            var countryType = GetTypeByCountryCode(countryCode);
            Type constructed = typeof(CovidStatConverter<>).MakeGenericType(countryType);
            var deserializedObject = DeserializeObject(jsonData, countryType, constructed);

            return (BaseCovidStatModel) deserializedObject;
        }

        private static object DeserializeObject(string jsonData, Type countryType, Type constructed)
            => JsonConvert.DeserializeObject(jsonData, countryType,
                (JsonConverter) Activator.CreateInstance(constructed) ??
                throw new NotSupportedException("Cannot instantiate country. It was not defined."));

        private Type GetTypeByCountryCode(string countryCode)
            => Type.GetType($"{_modelsNamespace}.{countryCode}")
               ?? throw new NotSupportedException("Can't match type for given country code");

        private string CutOffRedundantData(string countryCode, string jsonData)
            => "{" + $"\"{countryCode}" + jsonData.Split(countryCode)[1];
    }
}