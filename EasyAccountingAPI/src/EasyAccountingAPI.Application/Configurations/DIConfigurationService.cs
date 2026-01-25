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

            #region Global Service
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            #endregion

            #region Authentication Service
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
            services.AddScoped<IUserLoginHistoryRepository, UserLoginHistoryRepository>();
            #endregion

            #region Master Settings Service
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IInvoiceSettingRepository, InvoiceSettingRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            #endregion

            return services;
        }
    }
}