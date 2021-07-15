using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropertyAds.WebApp.Data;
using PropertyAds.WebApp.Data.Models;
using PropertyAds.WebApp.Services;

namespace PropertyAds.WebApp.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        private static void ConfigureIdentityOptions(IdentityOptions options)
        {
                options.SignIn.RequireConfirmedAccount = false;
        }

        public static IServiceCollection AddTransientServices(this IServiceCollection services)
        {
            services
                .AddTransient<IDistrictData, DistrictData>()
                .AddTransient<IPropertyTypeData, PropertyTypeData>()
                .AddTransient<IPropertyData, PropertyData>();

            return services;
        }

        public static IServiceCollection SetupIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<User>(ConfigureIdentityOptions)
                .AddEntityFrameworkStores<PropertyAdsDbContext>();

            return services;
        }

        public static IServiceCollection SetupDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PropertyAdsDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
