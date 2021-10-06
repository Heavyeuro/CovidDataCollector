using CovidDataCollector.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CovidDataCollector.Converters
{
    public class CovidStatConverter<T> : JsonCreationConverter<T> where T : BaseCovidStatModel
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected override T Create(Type objectType, JObject jObject)
        {
            if (!jObject.TryGetValue(objectType.Name, StringComparison.OrdinalIgnoreCase, out var resourceTypeToken))
            {
                throw new NotSupportedException($"Country without 'class' property are not supported. The object was {jObject}");
            }

            return (T)JsonConvert.DeserializeObject(resourceTypeToken.ToString(), objectType);
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jObject);
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override bool CanWrite => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            T target = Create(objectType, jObject);
            return target;
        }
    }
}
