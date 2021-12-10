using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CovidDataCollector.Models;
using CsvHelper;

namespace CovidDataCollector.Services
{
    public class XlsService
    {
        private static readonly string root = Directory.GetCurrentDirectory() + "\\..\\CsvStorage\\";
        private static readonly string RealDataPath = root + "covidData.csv";
        private static readonly string DailyPredictionFileName = root + DateTime.Today.ToString("yyyy-M-d") + ".csv";


        public static List<DailyCovidStatModel> ReadCsv()
        {
            using var reader = new StreamReader(RealDataPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = new List<DailyCovidStatModel>();
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {

                records.Add(new DailyCovidStatModel
                {
                    date = csv.GetField<string>("date"),
                    new_cases = csv.GetField<double>("new_cases"),
                    new_deaths = csv.GetField<double?>("new_deaths"),
                });
            }

            return records.ToList(); 
        }

        public static List<double> ReadCsvDoubleCol()
        {
            using var reader = new StreamReader(DailyPredictionFileName);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<double>().ToList();
        }
    }
}