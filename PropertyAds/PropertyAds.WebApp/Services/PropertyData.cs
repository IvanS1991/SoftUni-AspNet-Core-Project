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

        public PropertyData(PropertyAdsDbContext db)
        {
            this.db = db;
        }

        public async Task<Property> Create(Property property)
        {
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
            return this.db.Properties
                .ToListAsync();
        }

        public Task<List<Property>> GetList(int limit, int offset)
        {
            return this.db.Properties
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }
    }
}
