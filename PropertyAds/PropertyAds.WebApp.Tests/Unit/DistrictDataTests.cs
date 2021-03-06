namespace PropertyAds.WebApp.Tests.Unit
{
    using AutoMapper;
    using NUnit.Framework;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.Utility;
    using PropertyAds.WebApp.Tests.Mocks;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class DistrictDataTests
    {
        private PropertyAdsDbContext db;
        private IMapper mapper;
        private ICache cache;
        private DistrictData districtData;
        private string testDistrict = "test-district";
    
        [SetUp]
        public void SetUp()
        {
            this.db = DatabaseMock.Instance();
            this.mapper = MapperMock.Instance();
            this.cache = CacheMock.Instance();
            this.districtData = new DistrictData(
                this.db,
                this.mapper,
                this.cache);
            this.testDistrict = "test-district";
        }

        [TearDown]
        public void TearDown()
        {
            this.db = null;
            this.mapper = null;
            this.districtData = null;
            this.testDistrict = null;
        }

        [Test]
        public async Task Create_ShouldAddToDatabase()
        {
            await this.districtData.Create(this.testDistrict);

            Assert.That(this.db.Districts.Count() == 1);
            Assert.That(this.db.Districts.First().Name == this.testDistrict);
        }

        [Test]
        public async Task Create_ShouldAddOnlyOnceWithSameName()
        {
            await this.districtData.Create(this.testDistrict);
            await this.districtData.Create(this.testDistrict);

            Assert.That(this.db.Districts.Count() == 1);
            Assert.That(this.db.Districts.First().Name == this.testDistrict);
        }

        [Test]
        public async Task Create_ShouldReturnServiceModel()
        {
            var result = await this.districtData.Create(this.testDistrict);

            Assert.IsInstanceOf<DistrictServiceModel>(result);
        }

        [Test]
        public async Task Exists_ShouldReturnCorrectBooleanValueByName()
        {
            string existingDistrictName = this.testDistrict;
            const string nonExistingDistrictName = "non-existing district";

            await this.districtData.Create(existingDistrictName);

            var exists = await this.districtData.Exists(existingDistrictName);
            var doesNotExist = await this.districtData.Exists(nonExistingDistrictName);

            Assert.That(this.db.Districts
                .Where(x => x.Name == existingDistrictName)
                .Any() == exists);
            Assert.That(this.db.Districts
                .Where(x => x.Name == nonExistingDistrictName)
                .Any() == doesNotExist);
        }

        [Test]
        public async Task Exists_ShouldReturnCorrectBooleanValueById()
        {
            string existingDistrictName = this.testDistrict;
            const string nonExistingDistrictId = "non-existing district";

            var result = await this.districtData.Create(existingDistrictName);

            var exists = await this.districtData.Exists(result.Id);
            var doesNotExist = await this.districtData.Exists(nonExistingDistrictId);

            Assert.That(this.db.Districts
                .Where(x => x.Id == result.Id)
                .Any() == exists);
            Assert.That(this.db.Districts
                .Where(x => x.Id == nonExistingDistrictId)
                .Any() == doesNotExist);
        }

        [Test]
        public async Task ByName_ShouldReturnCorrectDistrict()
        {
            var first = await this.districtData.Create("first");
            await this.districtData.Create("second");
            await this.districtData.Create("third");

            var result = await this.districtData.ByName("first");

            Assert.IsInstanceOf<DistrictServiceModel>(result);
            Assert.AreEqual(first.Id, result.Id);
            Assert.AreEqual(first.Name, result.Name);

            var nullResult = await this.districtData.ByName("fourth");

            Assert.AreEqual(null, nullResult);
        }

        [Test]
        public async Task All_ShouldReturnAllInCorrectOrder()
        {
            var districtNames = new List<string>()
            {
                "first", "second", "third", "fourth"
            };
            var orderedNames = districtNames
                .OrderBy(x => x)
                .ToList();

            foreach (var name in districtNames)
            {
                await this.districtData.Create(name);
            }

            var result = await this.districtData.All();
            var resultNames = result.Select(x => x.Name);

            Assert.AreEqual(orderedNames, resultNames);

            foreach (var district in result)
            {
                Assert.IsInstanceOf<DistrictServiceModel>(district);
            }
        }
    }
}
