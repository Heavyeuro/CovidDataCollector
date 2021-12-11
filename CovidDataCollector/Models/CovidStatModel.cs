namespace CovidDataCollector.Models
{
    public class CovidStatModel
    {
        public double total_cases { get; set; }
        public double new_cases { get; set; }
        public double? total_deaths { get; set; }
        public double? new_deaths { get; set; }
    }
}
