using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DL.SyncAPI.Models
{
    public class AppSettings
    {
        public static void Register(IConfigurationRoot config, IServiceCollection services, string appDirectory)
        {
            try
            {
                var appSettings = config.GetSection("AppSettings").Get<AppSettings>();
                services.AddSingleton(appSettings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

    }


}