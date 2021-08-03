namespace PropertyAds.WebApp.Tests.Services
{
    using AutoMapper;
    using Microsoft.Extensions.Configuration;
    using NUnit.Framework;
    using PropertyAds.Scraper;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyAggregateServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Tests.Mocks;

    [TestFixture]
    class PropertyAggregateDataTests
    {
        private PropertyAdsDbContext db;
        private IPropertyAggregateScraper scraper;
        private IPropertyTypeData propertyTypeData;
        private IDistrictData districtData;
        private IConfiguration config;
        private IMapper mapper;
        private PropertyAggregateData propertyAggregateData;

        [SetUp]
        public void SetUp()
        {
            this.db = DatabaseMock.Instance();
            this.mapper = MapperMock.Instance();
            this.propertyAggregateData = new PropertyAggregateData(
                this.db,
                this.scraper,
                this.propertyTypeData,
                this.districtData,
                this.config,
                this.mapper);
        }

        [TearDown]
        public void TearDown()
        {
            this.db = null;
            this.mapper = null;
            this.propertyAggregateData = null;
        }
    }
}
