using System.Collections.Generic;
using CovidDataCollector.Models;

namespace CovidDataCollector.Services
{
    public interface IPlotService
    {
        public string BuildNewCasesWithPredictionPlot();
        public string BuildDeathsPlot();
        public List<DailyCovidStatModel> GetTableData();
    }
}
