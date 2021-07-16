namespace PropertyAds.WebApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.PropertyType;
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

        public async Task Create(PropertyType propertyType)
        {
            if (await this.Exists(propertyType.Name))
            {
                return;
            }

            await this.db.PropertyTypes.AddAsync(propertyType);
            await this.db.SaveChangesAsync();
        }

        public Task<bool> Exists(string propertyTypeName)
        {
            return this.db.PropertyTypes
                .AnyAsync(x => x.Name == propertyTypeName);
        }

        public Task<List<PropertyTypeViewModel>> GetAll()
        {
            return this.db.PropertyTypes
                .Select(x => new PropertyTypeViewModel { })
                .ToListAsync();
        }
    }
}
