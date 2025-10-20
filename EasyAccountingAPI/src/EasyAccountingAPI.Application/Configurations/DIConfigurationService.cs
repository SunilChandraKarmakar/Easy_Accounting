namespace EasyAccountingAPI.Application.Configurations
{
    public static class DIConfigurationService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AutomapperMappingProfile>();
            }, Assembly.GetExecutingAssembly());

            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            // HttpContextAccessor
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