namespace PropertyAds.WebApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.Property;
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

        public async Task Create(Property property)
        {
            await this.db.Properties.AddAsync(property);
            await this.db.SaveChangesAsync();
        }

        public Task Update(Property property)
        {
            throw new NotImplementedException();
        }

        public Task<List<PropertySummaryViewModel>> GetList()
        {
            return this.db.Properties
                .Select(x => new PropertySummaryViewModel { })
                .ToListAsync();
        }

        public Task<List<PropertySummaryViewModel>> GetList(int limit, int offset)
        {
            return this.db.Properties
                .Skip(offset)
                .Take(limit)
                .Select(x => new PropertySummaryViewModel { })
                .ToListAsync();
        }
    }
}
