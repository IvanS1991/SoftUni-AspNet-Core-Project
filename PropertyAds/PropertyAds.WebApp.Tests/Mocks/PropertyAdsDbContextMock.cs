namespace PropertyAds.WebApp.Tests.Mocks
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class PropertyAdsDbContextMock
    {
        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            return dbSet.Object;
        }

        private static DbSet<User> GetMockUsers()
        {
            List<User> users = Enumerable.Range(0, 3)
                .Select(x => new User
                {
                    UserName = $"test-user1{x}",
                    PasswordHash = $"password-hash{x}",
                    Email = $"test-user1{x}"
                }).ToList();

            return GetQueryableMockDbSet(users);
        }

        private static DbSet<Property> GetMockProperties(IEnumerable<District> districts, IEnumerable<PropertyType> propertyTypes)
        {
            List<Property> properties = Enumerable.Range(0, 3)
                .Select(x => new Property
                {
                    Area = 50 + x,
                    Price = 50000 + 10000 * x,
                    DistrictId = districts.Skip(x).First().Id,
                    TypeId = propertyTypes.Skip(x).First().Id,
                }).ToList();

            return GetQueryableMockDbSet(properties);
        }

        private static DbSet<District> GetMockDistricts()
        {
            List<District> districts = Enumerable.Range(0, 3)
                .Select(x => new District
                {
                    Name = $"district-{x}"
                }).ToList();

            return GetQueryableMockDbSet(districts);
        }

        private static DbSet<PropertyType> GetMockPropertyTypes()
        {
            List<PropertyType> districts = Enumerable.Range(0, 3)
                .Select(x => new PropertyType
                {
                    Name = $"property-type-{x}"
                }).ToList();

            return GetQueryableMockDbSet(districts);
        }

        private static DbSet<PropertyAggregate> GetPropertyAggregatesMock(IEnumerable<District> districts, IEnumerable<PropertyType> propertyTypes)
        {
            List<PropertyAggregate> propertyAggregates = Enumerable.Range(0, 3)
                .Select(x => new PropertyAggregate
                {
                }).ToList();

            return GetQueryableMockDbSet(propertyAggregates);
        }

        public static Mock<PropertyAdsDbContext> Get()
        {
            var db = new Mock<PropertyAdsDbContext>();
            var districts = GetMockDistricts();
            var propertyTypes = GetMockPropertyTypes();

            db.Setup(p => p.Set<User>())
                .Returns(GetMockUsers());
            db.Setup(p => p.Set<Property>())
                .Returns(GetMockProperties(districts.AsEnumerable(), propertyTypes.AsEnumerable()));
            db.Setup(p => p.Set<District>())
                .Returns(districts);
            db.Setup(p => p.Set<PropertyType>())
                .Returns(propertyTypes);
            db.Setup(p => p.Set<PropertyAggregate>())
                .Returns(GetPropertyAggregatesMock(districts.AsEnumerable(), propertyTypes.AsEnumerable()));

            db.Setup(p => p.SaveChanges()).Returns(1);

            return db;
        }
    }
}
