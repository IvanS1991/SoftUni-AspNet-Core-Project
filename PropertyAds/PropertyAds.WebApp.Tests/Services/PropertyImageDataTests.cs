namespace PropertyAds.WebApp.Tests.Services
{
    using AutoMapper;
    using NUnit.Framework;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Tests.Mocks;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class PropertyImageDataTests
    {
        private PropertyAdsDbContext db;
        private IMapper mapper;
        private PropertyImageData propertyImageData;

        private byte[] GetBytes()
        {
            return Enumerable.Range(0, 255)
                .Select(x => (byte)x).ToArray();
        }

        private void PopulateProperties()
        {
            this.db.Properties.Add(new Property());
            this.db.SaveChanges();
        }

        [SetUp]
        public void SetUp()
        {
            this.db = DatabaseMock.Instance();
            this.mapper = MapperMock.Instance();
            this.propertyImageData = new PropertyImageData(this.db, this.mapper);
        }

        [TearDown]
        public void TearDown()
        {
            this.db = null;
            this.mapper = null;
            this.propertyImageData = null;
        }

        [Test]
        public async Task Create_ShouldAddDbEntries()
        {
            this.PopulateProperties();

            var property = this.db.Properties.First();

            var expectedFileCount = 3;

            for (int i = 0; i < expectedFileCount; i++)
            {
                var fileName = $"file-${i}";
                var result = await this.propertyImageData.Create(GetBytes(), fileName, property.Id);

                Assert.IsInstanceOf<PropertyImageServiceModel>(result);
                Assert.AreEqual(fileName, result.Name);
            }

            Assert.AreEqual(this.db.PropertyImages.Count(), expectedFileCount);
        }

        [Test]
        public async Task GetById_ShouldReturnCorrectImage()
        {
            this.PopulateProperties();

            var property = this.db.Properties.First();

            var expectedResult = await this.propertyImageData.Create(GetBytes(), "asdf", property.Id);

            for (int i = 0; i < 3; i++)
            {
                var fileName = $"file-${i}";
               await this.propertyImageData.Create(GetBytes(), fileName, property.Id);
            }

            var result = await this.propertyImageData.GetById(expectedResult.Id);

            Assert.AreEqual(result.Id, expectedResult.Id);
            Assert.AreEqual(result.Name, expectedResult.Name);
        }
    }
}
