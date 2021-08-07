namespace PropertyAds.WebApp.Tests.Services
{
    using System;
    using AutoMapper;
    using Microsoft.Extensions.Configuration;
    using NUnit.Framework;
    using PropertyAds.Scraper;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyAggregateServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Tests.Mocks;
    using System.Threading.Tasks;
    using System.Linq;
    using Moq;
    using System.Collections.Generic;
    using PropertyAds.WebApp.Services.Utility;

    [TestFixture]
    public class PropertyAggregateDataTests
    {
        private PropertyAdsDbContext db;
        private IPropertyAggregateScraper propertyAggregateScraper;
        private IPropertyTypeData propertyTypeData;
        private IDistrictData districtData;
        private IConfiguration config;
        private IMapper mapper;
        private ICache cache;
        private PropertyAggregateData propertyAggregateData;

        [SetUp]
        public void SetUp()
        {
            this.db = DatabaseMock.Instance();
            this.propertyAggregateScraper = PropertyAggregateScraperMock.Instance();
            this.mapper = MapperMock.Instance();
            this.cache = CacheMock.Instance();
            this.config = ConfigurationMock.Instance();
            this.propertyTypeData = new PropertyTypeData(this.db, this.mapper, this.cache);
            this.districtData = new DistrictData(this.db, this.mapper, this.cache);

            var propertyAggregateData = new Mock<PropertyAggregateData>(
                this.db,
                this.propertyAggregateScraper,
                this.propertyTypeData,
                this.districtData,
                this.config,
                this.mapper,
                this.cache);

            propertyAggregateData.Setup(x => x.GetItemsPerPage())
                .Returns(2);

            this.propertyAggregateData = propertyAggregateData.Object;
        }

        [TearDown]
        public void TearDown()
        {
            this.db = null;
            this.mapper = null;
            this.propertyAggregateData = null;
        }

        [Test]
        public async Task Create_ShouldAddCorrectEntityAndReturnResult()
        {
            const int averagePrice = 200000;
            const int averagePricePerSqM = 2000;

            var propertyType = await this.propertyTypeData.Create("test_property_type", 0);
            var district = await this.districtData.Create("test_district");

            var result = this.db.PropertyAggregates
                .FirstOrDefault(x => x.District.Id == district.Id
                    && x.PropertyType.Id == propertyType.Id);

            Assert.IsNull(result);

            var methodResult = await this.propertyAggregateData.Create(
                district.Id,
                propertyType.Id,
                averagePrice,
                averagePricePerSqM);

            result = this.db.PropertyAggregates
                .FirstOrDefault(x => x.DistrictId == district.Id
                    && x.PropertyTypeId == propertyType.Id);

            Assert.NotNull(result);
            Assert.IsInstanceOf<PropertyAggregateServiceModel>(methodResult);
            Assert.AreEqual(methodResult.PropertyType.Id, result.PropertyType.Id);
            Assert.AreEqual(methodResult.District.Id, result.District.Id);
            Assert.AreEqual(methodResult.AveragePrice, result.AveragePrice);
            Assert.AreEqual(methodResult.AveragePricePerSqM, result.AveragePricePerSqM);
        }

        [Test]
        public void Create_ShouldThrowIfPropertyTypeOrDistrictDoesNotExist()
        {
            var exception = Assert.ThrowsAsync<ArgumentException>(() => this.propertyAggregateData.Create(
                "non-existing-district",
                "non-existing-property-type",
                20000,
                2000));

            Assert.AreEqual(exception.Message, "District or property type does not exist");
        }

        [Test]
        public async Task Populate_ShouldPopulateDbCorrectly()
        {
            var mockAggregateData = PropertyAggregateScraperMock.GetMockData();
            var propertyTypeCount = mockAggregateData.Aggregates
                .Select(x => x.DistrictName)
                .Distinct()
                .Count();
            var districtCount = mockAggregateData.Aggregates
                .Select(x => x.DistrictName)
                .Distinct()
                .Count();

            await this.propertyAggregateData
                .Populate();

            Assert.AreEqual(this.db.PropertyTypes.Count(), propertyTypeCount);
            Assert.AreEqual(this.db.Districts.Count(), districtCount);

            Assert.AreEqual(this.db.PropertyAggregates.Count(), mockAggregateData.Aggregates.Count());

            foreach (var aggregate in mockAggregateData.Aggregates)
            {
                var result = this.db.PropertyAggregates
                    .FirstOrDefault(x => x.District.Name == aggregate.DistrictName
                        && x.PropertyType.Name == aggregate.PropertyTypeName
                        && x.AveragePrice == aggregate.AveragePrice
                        && x.AveragePricePerSqM == aggregate.AveragePricePerSqM);

                Assert.NotNull(result);
            }
        }

        [Test]
        public async Task GetCount_ShouldReturnCorrectCountWithFiltering()
        {
            var mockAggregateData = PropertyAggregateScraperMock.GetMockData();

            await this.propertyAggregateData
                .Populate();

            var firstPropertyType = this.db.PropertyTypes.First();
            var firstDistrict = this.db.Districts.First();

            var count = await this.propertyAggregateData
                .GetCount(firstDistrict.Id, firstPropertyType.Id);

            var expectedCount = this.db.PropertyAggregates
                .Where(x => x.DistrictId == firstDistrict.Id
                    && x.PropertyTypeId == firstPropertyType.Id)
                .Count();

            Assert.AreEqual(count, expectedCount);
        }

        [Test]
        public async Task GetAll_ShouldReturnCorrectListWithFiltering()
        {
            var mockAggregateData = PropertyAggregateScraperMock.GetMockData();

            await this.propertyAggregateData
                .Populate();

            var firstPropertyType = this.db.PropertyTypes.First();
            var firstDistrict = this.db.Districts.First();

            var result = await this.propertyAggregateData
                .GetAll(0, firstDistrict.Id, firstPropertyType.Id);

            var expectedCount = this.db.PropertyAggregates
                .Where(x => x.DistrictId == firstDistrict.Id
                    && x.PropertyTypeId == firstPropertyType.Id)
                .Count();

            Assert.AreEqual(result.Count(), expectedCount);

            foreach (var aggregate in result)
            {
                Assert.IsInstanceOf<PropertyAggregateServiceModel>(aggregate);
            }
        }

        [Test]
        public async Task GetAll_ShouldReturnCorrectListWithPagination()
        {
            await this.propertyAggregateData
                .Populate();

            var firstPage = await this.propertyAggregateData
                .GetAll(0, null, null);
            var secondPage = await this.propertyAggregateData
                .GetAll(0, null, null);

            Assert.AreNotEqual(firstPage, secondPage);
        }

        [Test]
        public async Task TotalPageCount_ShouldReturnCorrectPageCount()
        {
            await this.propertyAggregateData
                .Populate();

            var result = await this.propertyAggregateData
                .TotalPageCount(null, null);

            Assert.AreEqual(result, 2);
        }
    }
}
