using System;
using CovidDataCollector.Models;
using Newtonsoft.Json;

namespace CovidDataCollector.Serializer
{
    public static class CovidStatSerializer
    {
        private static readonly string ModelsNamespace = typeof(BaseCovidStatModel).Namespace;
        public static string SerializeObject(BaseCovidStatModel covidStatModel)
        {
            return JsonConvert.SerializeObject(covidStatModel);
        }

        public static BaseCovidStatModel DeserializeJson(string countryCode, string jsonData)
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

        private static Type GetTypeByCountryCode(string countryCode)
            => Type.GetType($"{ModelsNamespace}.{countryCode}")
               ?? throw new NotSupportedException("Can't match type for given country code");
    }
}
