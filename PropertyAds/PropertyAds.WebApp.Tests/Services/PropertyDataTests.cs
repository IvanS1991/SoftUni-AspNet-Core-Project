namespace PropertyAds.WebApp.Tests.Services
{
    using AutoMapper;
    using Moq;
    using NUnit.Framework;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Services.UserServices;
    using PropertyAds.WebApp.Tests.Mocks;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class PropertyDataTests
    {
        private Random random;
        private PropertyAdsDbContext db;
        private IMapper mapper;
        private IUserData userData;
        private IPropertyImageData propertyImageData;
        private PropertyData propertyData;
        private string userEmail = "gosho@abv.bg";

        [SetUp]
        public void SetUp()
        {
            this.random = new Random();
            this.db = DatabaseMock.Instance();
            this.mapper = MapperMock.Instance();
            this.userData = UserDataMock.Instance();
            this.propertyImageData = new PropertyImageData(this.db, this.mapper);
            
            var propertyData = new Mock<PropertyData>(
                this.db,
                this.userData,
                MapperMock.Instance(),
                ConfigurationMock.Instance(),
                this.propertyImageData);

            propertyData.Setup(x => x.GetItemsPerPage())
                .Returns(2);

            this.propertyData = propertyData.Object;
        }

        [TearDown]
        public void TearDown()
        {
            this.db = null;
            this.userData = null;
            this.propertyData = null;
        }

        [Test]
        public async Task Create_ShouldCreatePropertyCorrectly()
        {
            this.PopulateDb();

            var price = 50000;
            var area = 50.45M;
            var usableArea = 40.45M;
            var floor = 2;
            var totalFloors = 5;
            var year = 2005;
            var description = "test-description";

            var insertedEntity = await this.propertyData.Create(
                price,
                area,
                usableArea,
                floor,
                totalFloors,
                year,
                description,
                this.db.PropertyTypes.First().Id,
                this.db.Districts.First().Id);

            var result = this.db.Properties
                .FirstOrDefault(x => x.Id == insertedEntity.Id);

            Assert.NotNull(result);
            this.AssertProperty(insertedEntity, result);
        }
        
        [Test]
        public async Task Delete_ShouldDeletePropertyAndCorrespondingImages()
        {
            this.PopulateDb();

            var firstProperty = this.db.Properties.First();
            var imageCount = 3;

            for (int i = 0; i < imageCount; i++) {
                await this.propertyImageData.Create(
                    new byte[] { },
                    $"test-image-{i}",
                    firstProperty.Id);
            }

            Assert.NotNull(this.db.Properties.Find(firstProperty.Id));
            Assert.AreEqual(
                this.GetPropertyImagesCount(firstProperty.Id),
                imageCount);

            await this.propertyData.Delete(firstProperty.Id);

            Assert.AreEqual(
                this.GetPropertyImagesCount(firstProperty.Id),
                0);
        }

        [Test]
        public async Task Update_ShouldUpdatePropertyCorrectly()
        {
            this.PopulateDb();

            var property = this.db.Properties.First();

            var price = 50000;
            var area = 50.45M;
            var usableArea = 40.45M;
            var floor = 3;
            var totalFloors = 5;
            var year = 2005;
            var description = Guid.NewGuid().ToString();
            var typeId = this.db.PropertyTypes.Last().Id;
            var districtId = this.db.Districts.Last().Id;

            var before = await this.propertyData.Find(property.Id);

            Assert.AreNotEqual(before.Price, price);
            Assert.AreNotEqual(before.Area, area);
            Assert.AreNotEqual(before.UsableArea, usableArea);
            Assert.AreNotEqual(before.Floor, floor);
            Assert.AreNotEqual(before.TotalFloors, totalFloors);
            Assert.AreNotEqual(before.Year, year);
            Assert.AreNotEqual(before.Description, description);
            Assert.AreNotEqual(before.Type.Id, typeId);
            Assert.AreNotEqual(before.District.Id, districtId);

            await this.propertyData.Update(
                property.Id,
                price,
                area,
                usableArea,
                floor,
                totalFloors,
                year,
                description,
                typeId,
                districtId);

            var after = await this.propertyData.Find(property.Id);

            Assert.AreEqual(after.Price, price);
            Assert.AreEqual(after.Area, area);
            Assert.AreEqual(after.UsableArea, usableArea);
            Assert.AreEqual(after.Floor, floor);
            Assert.AreEqual(after.TotalFloors, totalFloors);
            Assert.AreEqual(after.Year, year);
            Assert.AreEqual(after.Description, description);
            Assert.AreEqual(after.Type.Id, typeId);
            Assert.AreEqual(after.District.Id, districtId);
        }

        [Test]
        public async Task GetList_ShouldGetAllWithNoArguments()
        {
            this.PopulateDb();
            
            var result = await this.propertyData.GetList(null, null);

            Assert.AreEqual(this.db.Properties.Count(), result.Count());

            foreach (var item in result)
            {
                Assert.IsInstanceOf<PropertyServiceModel>(item);
            }
        }

        [Test]
        public async Task GetList_ShouldGetCorrectlyWithPagination()
        {
            this.PopulateDb();

            var firstResult = await this.propertyData.GetList(1, null, null);
            var secondResult = await this.propertyData.GetList(2, null, null);

            Assert.AreNotEqual(firstResult, secondResult);
        }

        [Test]
        public async Task Find_ShouldGetCorrectProperty()
        {
            this.PopulateDb();

            var lastProperty = this.db.Properties
                .Last();

            var result = await this.propertyData
                .Find(lastProperty.Id);

            Assert.NotNull(result);
            this.AssertProperty(result, lastProperty);
        }

        [Test]
        public async Task VisitProperty_ShouldIncreaseViewCountCorrectly()
        {
            this.PopulateDb();

            var lastProperty = this.db.Properties
                .Last();
            PropertyServiceModel result = null;

            Assert.AreEqual(lastProperty.VisitedCount, 0);

            for (int i = 0; i < 3; i++)
            {
                result = await this.propertyData
                    .VisitProperty(lastProperty.Id);
            }

            Assert.AreEqual(result.VisitedCount, 3);
        }

        private int GetPropertyImagesCount(string propertyId)
        {
            return this.db.PropertyImages
                .Where(x => x.PropertyId == propertyId)
                .Count();
        }

        private void AssertProperty(PropertyServiceModel insertedEntity, Property result)
        {
            Assert.IsInstanceOf<PropertyServiceModel>(insertedEntity);
            Assert.AreEqual(insertedEntity.Price, result.Price);
            Assert.AreEqual(insertedEntity.Area, result.Area);
            Assert.AreEqual(insertedEntity.UsableArea, result.UsableArea);
            Assert.AreEqual(insertedEntity.Floor, result.Floor);
            Assert.AreEqual(insertedEntity.TotalFloors, result.TotalFloors);
            Assert.AreEqual(insertedEntity.Year, result.Year);
            Assert.AreEqual(insertedEntity.Description, result.Description);
            Assert.AreEqual(insertedEntity.Type.Name, result.Type.Name);
            Assert.AreEqual(insertedEntity.District.Name, result.District.Name);
            Assert.AreEqual(insertedEntity.Owner, this.userEmail);
        }

        private void PopulateDb()
        {
            this.db.Users
                .Add(new User
                {
                    Id = this.userData.GetCurrentUserId(),
                    Email = this.userEmail
                });

            for (int i = 0; i < 10; i++)
            {
                var propertyType = this.db.PropertyTypes
                    .Add(new PropertyType
                    {
                        Name = $"test-property-type-{i}"
                    });

                var district = this.db.Districts
                    .Add(new District
                    {
                        Name = $"test-district-{i}"
                    });

                this.db.Properties.Add(new Property
                {
                    Price = this.random.Next(20000, 50000),
                    Area = this.random.Next(50, 60),
                    UsableArea = this.random.Next(40, 50),
                    Floor = this.random.Next(2, 3),
                    TotalFloors = this.random.Next(4, 5),
                    Year = this.random.Next(2000, 2005),
                    Description = "test-description",
                    CreatedOn = DateTime.UtcNow,
                    OwnerId = this.userData.GetCurrentUserId(),
                    TypeId = propertyType.Entity.Id,
                    DistrictId = district.Entity.Id,
                });
            }

            this.db.SaveChanges();
        }
    }
}
