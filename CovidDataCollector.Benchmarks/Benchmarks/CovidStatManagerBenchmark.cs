using BenchmarkDotNet.Attributes;
using CovidDataCollector.Managers;

namespace CovidDataCollector.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.SlowestToFastest)]
    [RankColumn]
    public class CovidStatManagerBenchmark
    {
        private readonly string _inputJson;
        private readonly CovidStatManager _reflectionCovidStatManager;
        private readonly CovidStatManagerSwitch _switchCovidStatManager;
        private const string countryCode = "UKR";

        public CovidStatManagerBenchmark()
        {
            _inputJson = System.IO.File.ReadAllText(@"D:\NetRepos\CovidDataCollector\CovidDataCollector.Benchmarks\CovidDataJson.txt");
            _reflectionCovidStatManager = new CovidStatManager();
            _switchCovidStatManager = new CovidStatManagerSwitch();
        }

        // Current implementation with reflection
        [Benchmark(Baseline = true)]
        public void ReflectionCovidStatManagerBenchmark()
        {
            _reflectionCovidStatManager.DeserializeJson(countryCode, _inputJson);
        }

        // Switch implementation without reflection
        [Benchmark]
        public void SwitchCovidStatManagerBenchmark()
        {
            _switchCovidStatManager.DeserializeJson(countryCode, _inputJson);
        }
    }
}
