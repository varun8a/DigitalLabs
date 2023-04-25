using DL.WebApp.Contracts;

namespace DL.WebApp.Models
{
    public class AppConfig : IAppConfig
    {
        public static void Register(IConfigurationRoot config, IServiceCollection services)
        {
            var appConfig = config.GetSection("AppConfig").Get<AppConfig>();
            services.AddSingleton<IAppConfig>(appConfig);
        }
        public string UrlCreateCustomer { get; set; }
        public string URLGetCustomers { get; set; }
    }
}

