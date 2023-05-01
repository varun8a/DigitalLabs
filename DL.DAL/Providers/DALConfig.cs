using Microsoft.Extensions.DependencyInjection;

namespace DL.DAL.Providers
{
    public static class DALConfig
    {
        /// <summary>
        /// Register DBops singleton object for DB initialization
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="services"></param>
        public static void Register(string connectionString, IServiceCollection services)
        {
            var dbOps = new DBOps(new DBProvider(connectionString));
            services.AddSingleton(dbOps);
        }
    }
}