namespace EasyAccountingAPI.Application.Configurations
{
    public static class DIConfigurationService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            // Add auto mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AutomapperMappingProfile>();
            }, Assembly.GetExecutingAssembly());

            // Add mediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            // Register IHttpContextAccessor 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Global services
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICountryManager, CountryManager>();

            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICityManager, CityManager>();

            return services;
        }
    }
}