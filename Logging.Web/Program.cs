using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging.AzureAppServices;
using Serilog;
using Serilog.Events;

var home = Environment.GetEnvironmentVariable("HOME");
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    //.WriteTo.File(new JsonFormatter(), @"Logs\log.txt", shared:true)
    .WriteTo.File(
       System.IO.Path.Combine(Environment.GetEnvironmentVariable("HOME"), "LogFiles", "Application", "diagnostics.txt"), // Create a user env variable to test on Windows Locally
       rollingInterval: RollingInterval.Day,
       fileSizeLimitBytes: 10 * 1024 * 1024,
       retainedFileCountLimit: 2,
       rollOnFileSizeLimit: true,f
       shared: true,
       flushToDiskInterval: TimeSpan.FromSeconds(1))
    .WriteTo.ApplicationInsights(new TelemetryConfiguration { InstrumentationKey = "Instrumentation_key" }, TelemetryConverter.Traces)
    .CreateLogger(); 

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(); // Include Serilog
  
    // Add services to the container.
    builder.Services.AddRazorPages();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Configure logging with a filter using builder.Host.ConfigureLogging
//builder.Logging.AddFilter((provider, category, logLevel) =>
//{
//    if (provider.Contains("ConsoleLoggerProvider")
//        && category.Contains("System")
//        && logLevel >= LogLevel.Information)
//    {
//        return true;
//    }
//    else if (provider.Contains("ConsoleLoggerProvider")
//        && category.Contains("Microsoft")
//        && logLevel >= LogLevel.Information)
//    {
//        return true;
//    }
//    else
//    {
//        return false;
//    }
//});


// Configure logging with a filter using builder.Host.ConfigureLogging
//builder.Host.ConfigureLogging(loggingBuilder =>
//{
//    loggingBuilder.AddFilter("Microsoft", LogLevel.Warning); // Filter out Microsoft logs below Warning level
//    loggingBuilder.AddFilter("System", LogLevel.Warning);   // Filter out System logs below Warning level
//    loggingBuilder.AddFilter(category: "YourNamespace", LogLevel.Information); // Adjust the namespace and log level as needed
//});


