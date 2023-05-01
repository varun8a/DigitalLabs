using Microsoft.Extensions.DependencyInjection;

namespace DL.DAL.Providers
{
    public static class DALConfig
    {
        public static void Register(string connectionString, IServiceCollection services)
        {
            var dbOps = new DBOps(new DBProvider(connectionString));
            services.AddSingleton(dbOps);
        }
    }
}