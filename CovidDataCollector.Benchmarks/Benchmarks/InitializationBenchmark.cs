using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

namespace CovidDataCollector.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.SlowestToFastest)]
    [RankColumn]
    public class InitializationBenchmark
    {
        private string countryCode = "UKR";
        //private readonly string ModelsNamespace = typeof(UKR).Namespace;

        // Current implementation with reflection
        [Benchmark(Baseline = true)]
        public void ReflectionInitializationBenchmark()
        {
            var ukr = new UKR();
        }

        // Switch implementation without reflection
        [Benchmark]
        public void NewInitializationBenchmark()
        {
            var ukr = Activator.CreateInstance(Type.GetType($"CovidDataCollector.Benchmarks.Benchmarks.UKR"));
        }

        public class UKR
        {
            public List<string> data { get; set; }
        }
    }
}
