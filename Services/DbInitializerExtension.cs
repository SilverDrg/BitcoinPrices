using BitcoinPrices.Models;

namespace BitcoinPrices.Services
{
    internal static class DbInitializerExtension
    {
        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<PricesContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {

            }

            return app;
        }
    }
}
