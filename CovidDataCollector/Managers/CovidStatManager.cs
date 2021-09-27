using CovidDataCollector.Models;
using Newtonsoft.Json;
using System;

namespace CovidDataCollector.Managers
{
    public class CovidStatManager : CovidStatManagerBase
    {
        // Generic implementation with reflection
        private readonly string ModelsNamespace;

        public CovidStatManager()
        {
            // "CovidDataCollector.Managers"
            ModelsNamespace = typeof(BaseCovidStatModel).Namespace;
        }

        public override BaseCovidStatModel DeserializeJson(string countryCode, string jsonData)
        {
            var countryType = GetTypeByCountryCode(countryCode);
            Type constructed = typeof(CovidStatConverter<>).MakeGenericType(countryType);
            var deserializedObject = JsonConvert.DeserializeObject(jsonData, countryType, (JsonConverter)Activator.CreateInstance(constructed));

            return (BaseCovidStatModel)deserializedObject;
        }

        private Type GetTypeByCountryCode(string countryCode)
            => Type.GetType($"{ModelsNamespace}.{countryCode}")
            ?? throw new NotSupportedException("Can't match type for given country code");
    }
}
