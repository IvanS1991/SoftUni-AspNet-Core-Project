namespace PropertyAds.WebApp.Tests.Services
{
    using AutoMapper;
    using NUnit.Framework;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Tests.Mocks;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class PropertyTypeDataTests
    {
        private PropertyAdsDbContext db;
        private IMapper mapper;
        private PropertyTypeData propertyTypeData;
        private string testDistrict = "test-property-type";

        [SetUp]
        public void SetUp()
        {
            this.db = DatabaseMock.Instance();
            this.mapper = MapperMock.Instance();
            this.propertyTypeData = new PropertyTypeData(this.db, this.mapper);
        }

        [TearDown]
        public void TearDown()
        {
            this.db = null;
            this.mapper = null;
            this.propertyTypeData = null;
            this.testDistrict = null;
        }

        [Test]
        public async Task Create_ShouldAddToDatabase()
        {
            await this.propertyTypeData.Create(this.testDistrict, 0);

            Assert.That(this.db.PropertyTypes.Count() == 1);
            Assert.That(this.db.PropertyTypes.First().Name == this.testDistrict);
        }

        [Test]
        public async Task Create_ShouldAddOnlyOnceWithSameName()
        {
            await this.propertyTypeData.Create(this.testDistrict, 0);
            await this.propertyTypeData.Create(this.testDistrict, 1);

            Assert.That(this.db.PropertyTypes.Count() == 1);
            Assert.That(this.db.PropertyTypes.First().Name == this.testDistrict);
        }

        [Test]
        public async Task Create_ShouldReturnServiceModel()
        {
            var result = await this.propertyTypeData
                .Create(this.testDistrict, 0);

            Assert.IsInstanceOf<PropertyTypeServiceModel>(result);
        }

        [Test]
        public async Task Exists_ShouldReturnCorrectBooleanValueByName()
        {
            string existingDistrictName = this.testDistrict;
            const string nonExistingDistrictName = "non-existing property type";

            await this.propertyTypeData.Create(existingDistrictName, 0);

            var exists = await this.propertyTypeData.Exists(existingDistrictName);
            var doesNotExist = await this.propertyTypeData.Exists(nonExistingDistrictName);

            Assert.That(this.db.PropertyTypes
                .Where(x => x.Name == existingDistrictName)
                .Any() == exists);
            Assert.That(this.db.PropertyTypes
                .Where(x => x.Name == nonExistingDistrictName)
                .Any() == doesNotExist);
        }

        [Test]
        public async Task Exists_ShouldReturnCorrectBooleanValueById()
        {
            string existingDistrictName = this.testDistrict;
            const string nonExistingDistrictId = "non-existing property type";

            var result = await this.propertyTypeData
                .Create(existingDistrictName, 0);

            var exists = await this.propertyTypeData.Exists(result.Id);
            var doesNotExist = await this.propertyTypeData.Exists(nonExistingDistrictId);

            Assert.That(this.db.PropertyTypes
                .Where(x => x.Id == result.Id)
                .Any() == exists);
            Assert.That(this.db.PropertyTypes
                .Where(x => x.Id == nonExistingDistrictId)
                .Any() == doesNotExist);
        }

        [Test]
        public async Task GetByName_ShouldReturnCorrectDistrict()
        {
            var first = await this.propertyTypeData.Create("first", 0);
            await this.propertyTypeData.Create("second", 1);
            await this.propertyTypeData.Create("third", 2);

            var result = await this.propertyTypeData.GetByName("first");

            Assert.IsInstanceOf<PropertyTypeServiceModel>(result);
            Assert.AreEqual(first.Id, result.Id);
            Assert.AreEqual(first.Name, result.Name);

            var nullResult = await this.propertyTypeData.GetByName("fourth");

            Assert.AreEqual(null, nullResult);
        }

        [Test]
        public async Task GetAll_ShouldReturnAllInCorrectOrder()
        {
            var districtNames = new List<string>()
            {
                "first", "second", "third", "fourth"
            };
            var orderedNames = districtNames
                .ToList();

            foreach (var name in districtNames)
            {
                await this.propertyTypeData.Create(name, 0);
            }

            var result = await this.propertyTypeData.GetAll();
            var resultNames = result.Select(x => x.Name);

            Assert.AreEqual(orderedNames, resultNames);

            foreach (var district in result)
            {
                Assert.IsInstanceOf<PropertyTypeServiceModel>(district);
            }
        }
    }
}
