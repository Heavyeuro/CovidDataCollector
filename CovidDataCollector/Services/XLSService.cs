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
        private static readonly string DailyPredictionFileName = root + DateTime.Today.ToString("yyyy-M-d");
        private static readonly string Extension = ".csv";


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
                    total_cases = csv.GetField<double>("total_cases"),
                    total_deaths = csv.GetField<double?>("total_deaths"),
                });
            }

            return records.ToList(); 
        }

        public static List<double> ReadCsvDoubleCol(string fileExtension)
        {
            using var reader = new StreamReader(DailyPredictionFileName + fileExtension + Extension);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<double>().ToList();
        }
    }
}