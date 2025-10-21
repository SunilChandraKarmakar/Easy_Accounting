namespace EasyAccountingAPI.Extensions
{
    public static class AppConfigureExtensions
    {
        public static WebApplication ConfigureCors(this WebApplication app, IConfiguration configuration)
        {
            app.UseCors("AllowAll");
            return app;
        }
    }
}