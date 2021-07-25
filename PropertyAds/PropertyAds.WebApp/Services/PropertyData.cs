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

        public async Task Update(Property property)
        {
            this.db.Properties.Update(property);
            await this.db.SaveChangesAsync();
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

        public Task<Property> Find(string query)
        {
            return this.db.Properties
                .Include(x => x.District)
                .Include(x => x.Type)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == query);
        }

        public async Task<Property> VisitProperty(string id)
        {
            var property = await this.Find(id);

            if (property == null)
            {
                return null;
            }

            property.VisitedCount += 1;

            await this.Update(property);

            return property;
        }
    }
}
