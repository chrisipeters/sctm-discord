using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace sctm.discord
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName)
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile("appsettings.secure.json", true,true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>()
                      .Configure<EventLogSettings>(config =>
                      {
                          config.SourceName = "ChrispyKangaroo";
                          config.LogName = "ChrispyKangaroo-DiscordBot";
                      });
                }).UseWindowsService();
    }
}
