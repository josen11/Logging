using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace ApplicationInsightsWithSDK
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            var channel1 = new ServerTelemetryChannel();
            services.Configure<TelemetryConfiguration>(
                (config) =>
                {
                    config.TelemetryChannel = channel1;
                }
            );

            services.AddLogging(builder =>
            {
                builder.AddApplicationInsights("32748b22-1791-4c74-89fe-6225b32744a7");
            });

            var provider = services.BuildServiceProvider();
            var logger = provider.GetService<ILogger<Program>>();
            int i = 0;
            while (true)
            {
                logger.LogInformation($"Hello From Console {++i}");
                Thread.Sleep(1000);
            }
        }
    }
}