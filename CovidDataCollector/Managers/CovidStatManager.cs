using CovidDataCollector.Converters;
using CovidDataCollector.Models;
using Newtonsoft.Json;
using System;

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
            var deserializedObject = DeserializeObject(jsonData, countryType, constructed);

            return (BaseCovidStatModel)deserializedObject;
        }

        private static object DeserializeObject(string jsonData, Type countryType, Type constructed)
            => JsonConvert.DeserializeObject(jsonData, countryType,
                (JsonConverter)Activator.CreateInstance(constructed) ??
                throw new NotSupportedException("Cannot instantiate country. It was not defined."));

        private Type GetTypeByCountryCode(string countryCode)
            => Type.GetType($"{_modelsNamespace}.{countryCode}")
               ?? throw new NotSupportedException("Can't match type for given country code");
    }
}