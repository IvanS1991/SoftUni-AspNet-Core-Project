namespace PropertyAds.WebApp.Tests.Mocks
{
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using System;

    public static class DatabaseMock
    {
        public static PropertyAdsDbContext Instance()
        {
            var options = new DbContextOptionsBuilder<PropertyAdsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new PropertyAdsDbContext(options);
        }
    }
}
