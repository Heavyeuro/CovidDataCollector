using BenchmarkDotNet.Running;
using CovidDataCollector.Benchmarks.Benchmarks;

namespace CovidDataCollector.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<InitializationBenchmark>();
        }


        //public class UKRAINE
        //{
        //    public List<string> data { get; set; }
        //    public void testMEthod()
        //    {
        //        var countryCode = "UKRAINE";
        //        Type q = new Type();
        //        var Namespace = q.Namespace;
        //        var path = Type.GetType($"{Namespace}.{countryCode}");
        //        var ukr = Activator.CreateInstance(path);
        //    }
        //}
    }
}
