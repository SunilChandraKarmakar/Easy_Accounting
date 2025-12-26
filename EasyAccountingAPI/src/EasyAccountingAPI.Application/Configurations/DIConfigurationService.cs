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

            // Unit of Work
            services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>(); 

            // Global services
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();

            return services;
        }
    }
}