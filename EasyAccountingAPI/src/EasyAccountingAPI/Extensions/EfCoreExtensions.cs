namespace EasyAccountingAPI.Extensions
{
    public static class EfCoreExtensions
    {
        public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Add database connection string
            services.AddDbContext<DatabaseContext>(option => option.UseSqlServer(configuration.GetConnectionString("Default")));
            return services;
        }
    }
}