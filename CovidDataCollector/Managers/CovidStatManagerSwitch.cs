using CovidDataCollector.Models;
using Newtonsoft.Json;

namespace CovidDataCollector.Managers
{
    public class CovidStatManagerSwitch : CovidStatManagerBase
    {
        // Implementation with simple switch
        public override BaseCovidStatModel DeserializeJson(string countryCode, string jsonData)
        {
            var deserializedObject = JsonConvert.DeserializeObject<BaseCovidStatModel>(jsonData, new SwitchCovidStatConverter());

            return deserializedObject;
        }
    }
}
