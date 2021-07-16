namespace PropertyAds.WebApp.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using PropertyAds.WebApp.Data;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
        {
            var servicesScope = app.ApplicationServices.CreateScope();
            var db = servicesScope.ServiceProvider.GetService<PropertyAdsDbContext>();

            db.Database.Migrate();

            return app;
        }
    }
}
