namespace EasyAccountingAPI.Application.Configurations.Seed
{
    public class ApplicationSeedConfig
    {
        public static async Task SeedAsync(DatabaseContext context, IMediator mediator)
        {
            // Check if seeding is needed for country and city
            if (!context.Countries.Any() && !context.Cities.Any())
            {
                var result = await mediator.Send(new CreateCountryCitySeedCommand());

                if (!result)
                {
                    throw new Exception("Seeding Country/City failed.");
                }
            }

            // Seed data for Super Admin
            await mediator.Send(new SuperAdminSeedCommand());

            // Seed data for Currency
            await mediator.Send(new CreateCurrencySeedCommand());

            // Seed data for Action
            await mediator.Send(new CreateActionSeedCommand());

            // Seed module and features
            await mediator.Send(new CreateFeatureSeedCommand());
        }
    }
}