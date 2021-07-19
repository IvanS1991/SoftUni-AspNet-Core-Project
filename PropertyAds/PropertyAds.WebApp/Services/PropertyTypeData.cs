namespace PropertyAds.WebApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PropertyTypeData : IPropertyTypeData
    {
        private readonly PropertyAdsDbContext db;

        public PropertyTypeData(PropertyAdsDbContext db)
        {
            this.db = db;
        }

        public async Task<PropertyType> Create(PropertyType propertyType)
        {
            var existingPropertyType = await this.GetByName(propertyType.Name);

            if (await this.Exists(propertyType.Name))
            {
                return existingPropertyType;
            }

            var result = await this.db.PropertyTypes.AddAsync(propertyType);
            await this.db.SaveChangesAsync();

            return result.Entity;
        }

        public Task<bool> Exists(string query)
        {
            return this.db.PropertyTypes
                .AnyAsync(x => x.Name == query || x.Id == query);
        }

        public Task<PropertyType> GetByName(string propertyTypeName)
        {
            return this.db.PropertyTypes
                .FirstOrDefaultAsync(x => x.Name == propertyTypeName);
        }

        public Task<List<PropertyType>> GetAll()
        {
            return this.db.PropertyTypes
                .OrderBy(x => x.SortRank)
                .ToListAsync();
        }
    }
}
