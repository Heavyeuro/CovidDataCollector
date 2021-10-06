using CovidDataCollector.Models;
using Newtonsoft.Json;
using System;
using CovidDataCollector.Converters;

namespace CovidDataCollector.Managers
{
    public class CovidStatManager : CovidStatManagerBase
    {
        // Generic implementation with reflection
        private readonly string _modelsNamespace;

        public CovidStatManager()
        {
            _modelsNamespace = typeof(BaseCovidStatModel).Namespace;
        }

        public override BaseCovidStatModel DeserializeJson(string countryCode, string jsonData)
        {
            var countryType = GetTypeByCountryCode(countryCode);
            Type constructed = typeof(CovidStatConverter<>).MakeGenericType(countryType);
            var deserializedObject = JsonConvert.DeserializeObject(jsonData, countryType,
                (JsonConverter) Activator.CreateInstance(constructed) ??
                throw new NotSupportedException("Cannot instantiate country.It was not defined."));

            return (BaseCovidStatModel) deserializedObject;
        }

        private Type GetTypeByCountryCode(string countryCode)
            => Type.GetType($"{_modelsNamespace}.{countryCode}")
               ?? throw new NotSupportedException("Can't match type for given country code");
    }
}