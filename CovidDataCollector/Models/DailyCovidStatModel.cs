namespace CovidDataCollector.Models
{
    public class DailyCovidStatModel
    {
        public string date { get; set; }
        public double total_cases { get; set; }
        public double new_cases { get; set; }
        public double total_cases_per_million { get; set; }
        public double new_cases_per_million { get; set; }
        public double stringency_index { get; set; }
        public double? new_cases_smoothed { get; set; }
        public double? new_deaths_smoothed { get; set; }
        public double? new_cases_smoothed_per_million { get; set; }
        public double? new_deaths_smoothed_per_million { get; set; }
        public double? total_deaths { get; set; }
        public double? new_deaths { get; set; }
        public double? total_deaths_per_million { get; set; }
        public double? new_deaths_per_million { get; set; }
        public double? reproduction_rate { get; set; }
        public double? excess_mortality_cumulative_absolute { get; set; }
        public double? excess_mortality_cumulative { get; set; }
        public double? excess_mortality { get; set; }
        public double? total_tests { get; set; }
        public double? total_tests_per_thousand { get; set; }
        public string tests_units { get; set; }
        public double? new_tests { get; set; }
        public double? new_tests_per_thousand { get; set; }
        public double? new_tests_smoothed { get; set; }
        public double? new_tests_smoothed_per_thousand { get; set; }
        public double? positive_rate { get; set; }
        public double? tests_per_case { get; set; }
    }
}
