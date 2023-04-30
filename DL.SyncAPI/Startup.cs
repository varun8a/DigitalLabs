using DL.SyncAPI;
using DL.SyncAPI.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DL.DAL.Providers;


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

            //AppSettings.Register(config, builder.Services, appDirectory);
            DALConfig.Register(config.GetConnectionString("DigitalLabsDB"), builder.Services);
            //builder.Services.AddHttpClient<IHttpClientHandler, HttpClientHandler>();
            builder.Services.AddOptions();
        }

    }
}