namespace EasyAccountingAPI.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerExplorer(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            // Register the Swagger generator
            services.AddOpenApiDocument(config =>
            {
                config.DocumentName = "v1";
                config.Title = "Easy Accounting Solution API";
                config.Version = "v1";

                // Add the processor to sort paths alphabetically
                config.DocumentProcessors.Add(new SortPathsAlphabeticallyProcessor());

                // Add JWT Bearer Authentication
                config.AddSecurity("Bearer", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
                {
                    Type = NSwag.OpenApiSecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Name = "Authorization",
                    Description = "Please, file in the JWT token"
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            return services;
        }

        public static WebApplication ConfigureSwaggerExplorer(this WebApplication app)
        {
            app.UseOpenApi();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Easy Accounting Solution API V1");
                c.DefaultModelsExpandDepth(2); // Set the default depth for expanding models
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // Change default expansion to uncollapsed
            });

            return app;
        }
    }
}