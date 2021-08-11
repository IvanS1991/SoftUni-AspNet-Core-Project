namespace PropertyAds.WebApp.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.PropertyAggregateServices;
    using System;
    using System.Threading.Tasks;

    using static PropertyAds.WebApp.Data.Roles;

    public static class ApplicationBuilderExtensions
    {
        private static void TryCreateAdminUser(IServiceProvider services)
        {
            var userManager = services.GetService<UserManager<User>>();
            var roleManager = services.GetService<RoleManager<IdentityRole>>();

            var email = "admin@property-ads.com";
            var password = "Admin@Property-Ads#2021";

            Task.Run(async () =>
            {
                var adminUser = await userManager.FindByEmailAsync(email);

                if (adminUser == null)
                {
                    adminUser = new User
                    {
                        Email = email,
                        UserName = email
                    };

                    await userManager.CreateAsync(adminUser, password);
                }

                if (await roleManager.RoleExistsAsync(Administrator) == false)
                {
                    await roleManager
                        .CreateAsync(new IdentityRole { Name = Administrator });
                }

                if (await userManager.IsInRoleAsync(adminUser, Administrator) == false)
                {
                    await userManager.AddToRoleAsync(adminUser, Administrator);
                }
            });
        }

        public static IApplicationBuilder UseDatabasePopulation(this IApplicationBuilder app, int? populateInterval)
        {
            var serviceProvider = app.ApplicationServices
                .CreateScope()
                .ServiceProvider;

            TryCreateAdminUser(serviceProvider);

            if (populateInterval != null && populateInterval > 0)
            {
                var propertyAggregateData = serviceProvider
                    .GetService<IPropertyAggregateData>();

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
