using System.Collections.Generic;

namespace CovidDataCollector.Models
{
    public class UKR : BaseCovidStatModel
    {
        public List<DailyCovidStatModel> data { get; set; }
    }

    public class AFG : BaseCovidStatModel
    {
        public List<DailyCovidStatModel> data { get; set; }
    }

    public class ITA : BaseCovidStatModel
    {
        public List<DailyCovidStatModel> data { get; set; }
    }

    public class RUS : BaseCovidStatModel
    {
        public List<DailyCovidStatModel> data { get; set; }
    }

    public class USA : BaseCovidStatModel
    {
        public List<DailyCovidStatModel> data { get; set; }
    }
}
