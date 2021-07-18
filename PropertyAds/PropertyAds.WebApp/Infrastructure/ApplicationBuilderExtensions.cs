namespace PropertyAds.WebApp.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Services;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabasePopulation(this IApplicationBuilder app, int? populateInterval)
        {
            if (populateInterval != null && populateInterval > 0)
            {
                var servicesScope = app.ApplicationServices.CreateScope();
                var propertyAggregateData = servicesScope.ServiceProvider.GetService<IPropertyAggregateData>();

                propertyAggregateData.RunPopulateTask((int)populateInterval);
            }

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
