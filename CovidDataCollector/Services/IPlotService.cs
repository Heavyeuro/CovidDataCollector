using System.Collections.Generic;
using CovidDataCollector.Models;

namespace CovidDataCollector.Services
{
    public interface IPlotService
    {
        public string BuildNewCasesWithPredictionPlot();
        public string BuildDeathsWithPredictionPlot();
        public List<DailyCovidStatModel> GetTableData();
    }
}
