using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using System.Threading;

namespace _11_console_serilog
{
    class Program
    {
        static void Main(string[] args)
        {
            using var log = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("file.txt")
                .WriteTo.ApplicationInsights(new TelemetryConfiguration { InstrumentationKey = "InstrumentationKey" }, TelemetryConverter.Traces)
                .CreateLogger();
            int i = 0;
            while (true)
            {
                log.Information($"Hello From Serilog! {++i}");
                Thread.Sleep(1000);
            }
        }
    }
}