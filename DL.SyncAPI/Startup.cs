using DL.SyncAPI;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using DL.DAL.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using DL.SyncAPI.Models;

[assembly: FunctionsStartup(typeof(Startup))]

namespace DL.SyncAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var appDirectory = builder.Services.BuildServiceProvider().GetService<IOptionsSnapshot<ExecutionContextOptions>>().Value.AppDirectory;
            var config = new ConfigurationBuilder()
                   .SetBasePath(appDirectory)
                   .AddJsonFile("appsettings.json", false)
                   .AddEnvironmentVariables()
                   .Build();


            builder.Services.AddApplicationInsightsTelemetry();
            
            builder.Services.Configure<AppConfig>(config.GetSection("AppConfig"));
            //Register DB
            DALConfig.Register(config.GetConnectionString("DigitalLabsDB"), builder.Services);
            builder.Services.AddOptions();

        }
    }
}