using System;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Challenge.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration().WriteTo.Console(theme: AnsiConsoleTheme.Code)
                   .MinimumLevel.Information()
                   .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                   .Enrich.WithProperty("AppName", "Challenge API")
                   .Enrich.WithProperty("Environment", "Production")
                   .Enrich.With(new ThreadIdEnricher())
                   .CreateLogger();

                Log.Information("Starting Web Host");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://0.0.0.0:25011");
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class ThreadIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
               "ThreadId", Thread.CurrentThread.ManagedThreadId));
        }
    }
}
