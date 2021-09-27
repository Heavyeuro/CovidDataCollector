using CovidDataCollector.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CovidDataCollector.Managers
{
    public class SwitchCovidStatConverter : JsonSwitchCreationConverter<BaseCovidStatModel>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected override BaseCovidStatModel Create(Type objectType, JObject jsonObject)
        {
            throw new NotImplementedException();
        }

    }

    public abstract class JsonSwitchCreationConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jObject);
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }
        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var type = GetCountryTypeName();


            if (!jObject.TryGetValue(type.Name, StringComparison.OrdinalIgnoreCase, out var resourceTypeToken))
            {
                throw new NotSupportedException($"Resource objects without 'resourceType' property are not supported. The object was {jObject}");
            }


            return JsonConvert.DeserializeObject(resourceTypeToken.ToString(), type);
        }

        protected Type GetCountryTypeName()
        {
            string countyCode = "UKR";

            return countyCode switch
            {
                "BASE" => typeof(BaseCovidStatModel),
                "USA" => typeof(USA),
                "ITA" => typeof(ITA),
                "RUS" => typeof(RUS),
                "AFG0" => typeof(AFG),
                "AFG1" => typeof(AFG),
                "AFG2" => typeof(AFG),
                "AFG3" => typeof(AFG),
                "AFG4" => typeof(AFG),
                "AFG5" => typeof(AFG),
                "AFG6" => typeof(AFG),
                "AFG7" => typeof(AFG),
                "UKR" => typeof(UKR),
                _ => throw new NotSupportedException(),
            };
        }
    }
}
