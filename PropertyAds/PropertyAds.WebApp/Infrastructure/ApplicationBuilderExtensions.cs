namespace PropertyAds.WebApp.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Services;
    using System.Threading.Tasks;
    using System.Timers;

    public static class ApplicationBuilderExtensions
    {
        private static async Task OnDatabasePopulate(IApplicationBuilder app)
        {
            var propertyAggregateData = app.ApplicationServices.GetService<IPropertyAggregateData>();

            await propertyAggregateData.Populate();
        }

        public static IApplicationBuilder UseDatabasePopulation(this IApplicationBuilder app, int populateInterval)
        {
            var timer = new Timer(populateInterval);

            timer.Elapsed += async (sender, e) => await OnDatabasePopulate(app);
            timer.AutoReset = true;
            timer.Enabled = true;

            return app;
        }

        public static IApplicationBuilder UseMigrations(this IApplicationBuilder app)
        {
            var servicesScope = app.ApplicationServices.CreateScope();
            var db = servicesScope.ServiceProvider.GetService<PropertyAdsDbContext>();

            db.Database.Migrate();

            return app;
        }
    }
}
