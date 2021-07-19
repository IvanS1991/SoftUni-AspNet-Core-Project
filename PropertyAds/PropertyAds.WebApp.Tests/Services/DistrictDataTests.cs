namespace PropertyAds.WebApp.Tests.Services
{
    using Moq;
    using NUnit.Framework;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services;
    using PropertyAds.WebApp.Tests.Mocks;
    using System.Threading.Tasks;

    [TestFixture]
    public class DistrictDataTests
    {
        private Mock<PropertyAdsDbContext> db;
        private DistrictData instance;
        private District testDistrict;

        [SetUp]
        public void SetUp()
        {
            this.db = PropertyAdsDbContextMock.Get();
            this.instance = new DistrictData(this.db.Object);
            this.testDistrict = new District
            {
                Name = "test-district"
            };
        }

        [TearDown]
        public void TearDown()
        {
            this.db = null;
            this.instance = null;
        }

        [Test]
        public async Task Create_ShouldAddAsyncAndSaveChanges()
        {
            await this.instance.Create(testDistrict);

            this.db.Verify(x => x.Districts.AddAsync(null, default), Times.Once);
        }
    }
}
