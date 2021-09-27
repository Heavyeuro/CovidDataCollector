using BenchmarkDotNet.Running;
using CovidDataCollector.Benchmarks.Benchmarks;

namespace CovidDataCollector.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<CovidStatManagerBenchmark>();
        }
    }
}
