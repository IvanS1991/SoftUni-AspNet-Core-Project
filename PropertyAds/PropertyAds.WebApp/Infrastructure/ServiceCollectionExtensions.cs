namespace PropertyAds.WebApp.Infrastructure
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using PropertyAds.Scraper;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.ConversationServices;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyAggregateServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Services.UserServices;
    using PropertyAds.WebApp.Services.Utility;
    using PropertyAds.WebApp.Services.WatchlistServices;

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
                .AddTransient<IPropertyData, PropertyData>()
                .AddTransient<IPropertyAggregateScraper, ImotBgScraper>()
                .AddTransient<IPropertyAggregateData, PropertyAggregateData>()
                .AddTransient<IWatchlistData, WatchlistData>()
                .AddTransient<IUserData, UserData>()
                .AddTransient<IPropertyImageData, PropertyImageData>()
                .AddTransient<IConversationData, ConversationData>()
                .AddTransient<ICache, Cache>();

            return services;
        }

        public static IServiceCollection AddScopedServices(this IServiceCollection services)
        {
            services
                .AddScoped<IDataFormatter, DataFormatter>();

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
