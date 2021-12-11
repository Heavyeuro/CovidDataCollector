using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CovidDataCollector.Managers;
using CovidDataCollector.Models;
using ScottPlot;

namespace CovidDataCollector.Services
{
    public class PlotService: IPlotService
    {
        private readonly ICovidStatManager _covidStatManager;

        public PlotService(ICovidStatManager covidStatManager)
        {
            _covidStatManager = covidStatManager;
        }

        public string BuildNewCasesWithPredictionPlot()
        {
            var realData = _covidStatManager.GetRealData();
            var predictedData = _covidStatManager.GetPredictionData("new_cases");

            var plotLines = new List<DailyPlotLine>
            {
                new("New cases", DateTime.Parse(realData[0].date),
                    realData.Select(x => x.new_cases).ToArray()),
                new("Predicted", DateTime.Today, predictedData.ToArray())
            };

            return MakePlot(plotLines, "", "Cases");
        }

        public string BuildDeathsWithPredictionPlot()
        {
            var realData = _covidStatManager.GetRealData();
            var predictedData = _covidStatManager.GetPredictionData("new_deaths");

            var plotLines = new List<DailyPlotLine>
            {
                new("Deaths per day", DateTime.Parse(realData[0].date),
                    realData.Select(x => x.new_deaths ?? 0).ToArray()),
                new("Predicted", DateTime.Today, predictedData.ToArray())
            };

            return MakePlot(plotLines, "", "Deaths");
        }

        public List<DailyCovidStatModel> GetTableData()
        {
            return _covidStatManager.GetRealData();
        }

        private static string MakePlot(List<DailyPlotLine> plotLines, string xLabel, string yLabel)
        {
            var path = Directory.GetCurrentDirectory() + $"\\..\\plots\\{Guid.NewGuid()}.png";

            var plt = new Plot();
            plt.XAxis.Label(xLabel);
            plt.YAxis.Label(yLabel);
            plt.XAxis.TickLabelFormat("MM/dd/yy", true);


            foreach (var plotLine in plotLines)
            {
                double[] days = new double[plotLine.YDots.Length];
                for (int i = 0; i < days.Length; i++)
                    days[i] = plotLine.StartDateTime.AddDays(1).AddDays(i).ToOADate();

                // plot the data with custom tick format (https://tinyurl.com/ycwh45af)
                plt.AddScatter(days, plotLine.YDots);
                plt.SetCulture("M\\/dd");
            }
           
            plt.Legend();

            plt.SaveFig(path);
            return path;
        }
    }

    public class DailyPlotLine
    {
        public string Label { get; set; }
        public double[] YDots { get; set; }
        public DateTime StartDateTime  { get; set; }

        public DailyPlotLine(string label, DateTime startDateTime, double[] yDots)
        {
            Label = label;
            StartDateTime = startDateTime;
            YDots = yDots;
        }
    }
}
