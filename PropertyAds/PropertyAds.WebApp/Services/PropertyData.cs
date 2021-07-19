namespace PropertyAds.WebApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PropertyData : IPropertyData
    {
        private readonly PropertyAdsDbContext db;
        private readonly IUserData userData;

        public PropertyData(
            PropertyAdsDbContext db,
            IUserData userData)
        {
            this.db = db;
            this.userData = userData;
        }

        public async Task<Property> Create(Property property)
        {
            property.CreatedOn = DateTime.UtcNow;
            property.OwnerId = this.userData.GetCurrentUserId();

            var result = await this.db.Properties.AddAsync(property);
            await this.db.SaveChangesAsync();

            return result.Entity;
        }

        public Task Update(Property property)
        {
            throw new NotImplementedException();
        }

        public Task<List<Property>> GetList()
        {
            return this.GetList(0);
        }

        public Task<List<Property>> GetList(int limit)
        {
            return this.GetList(limit, 0);
        }

        public Task<List<Property>> GetList(int limit, int offset)
        {
            var queryable = this.db.Properties
                .Include(x => x.District)
                .Include(x => x.Type)
                .Skip(offset);

            if (limit > 0)
            {
                queryable = queryable.Take(limit);
            }

            return queryable
                .ToListAsync();
        }
    }
}
