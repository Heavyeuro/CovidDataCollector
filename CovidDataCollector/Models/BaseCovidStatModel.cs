using System.Collections.Generic;

namespace CovidDataCollector.Models
{
    public class BaseCovidStatModel
    {
        public string continent { get; set; }
        public string location { get; set; }
        public double population { get; set; }
        public double population_density { get; set; }
        public double median_age { get; set; }
        public double aged_65_older { get; set; }
        public double aged_70_older { get; set; }
        public double gdp_per_capita { get; set; }
        public double extreme_poverty { get; set; }
        public double cardiovasc_death_rate { get; set; }
        public double diabetes_prevalence { get; set; }
        public double female_smokers { get; set; }
        public double male_smokers { get; set; }
        public double hospital_beds_per_thousand { get; set; }
        public double life_expectancy { get; set; }
        public double human_development_index { get; set; }
        public List<DailyCovidStatModel> data { get; set; }

    }
}
