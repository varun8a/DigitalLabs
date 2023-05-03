using Azure.Identity;
using DL.DAL.Providers;
using DL.SyncAPI;
using DL.SyncAPI.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace DL.SyncAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var appDirectory = builder.Services.BuildServiceProvider().GetService<IOptionsSnapshot<ExecutionContextOptions>>().Value.AppDirectory;
            builder.Services.AddApplicationInsightsTelemetry();

            builder.Services.AddOptions<AppConfig>()
                  .Configure<IConfiguration>((settings, configuration) =>
                  {
                      configuration.GetSection("AppConfig").Bind(settings);
                  });

            var config = new ConfigurationBuilder()
                   .SetBasePath(appDirectory)
                   .AddJsonFile("appsettings.json", false)
                   .AddEnvironmentVariables()
                   .Build();

            DALConfig.Register(config.GetConnectionString("DigitalLabsDB"), builder.Services);
            builder.Services.AddLogging();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var builtConfig = builder.ConfigurationBuilder.Build();
            var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];
            var managedIdentityObjectId = builtConfig["ManagedIdentityObjectId"];
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = managedIdentityObjectId });

            builder.ConfigurationBuilder
                    .AddAzureKeyVault(new Uri(keyVaultEndpoint), credential)
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("local.settings.json", true)
                    .AddEnvironmentVariables()
                .Build();
        }
    }
}