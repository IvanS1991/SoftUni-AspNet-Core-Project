namespace PropertyAds.WebApp.Services.PropertyServices
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

        private static PropertyTypeServiceModel FromDbModel(PropertyType dbModel)
        {
            return new PropertyTypeServiceModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name
            };
        }

        public async Task<PropertyTypeServiceModel> Create(string name, int sortRank)
        {
            var existingPropertyType = await this.GetByName(name);

            if (await this.Exists(name))
            {
                return existingPropertyType;
            }

            var result = await this.db.PropertyTypes.AddAsync(new PropertyType {
                Name = name,
                SortRank = sortRank
            });
            await this.db.SaveChangesAsync();

            return FromDbModel(result.Entity);
        }

        public Task<bool> Exists(string query)
        {
            return this.db.PropertyTypes
                .AnyAsync(x => x.Name == query || x.Id == query);
        }

        public async Task<PropertyTypeServiceModel> GetByName(string propertyTypeName)
        {
            var propertyType = await this.db.PropertyTypes
                .FirstOrDefaultAsync(x => x.Name == propertyTypeName);

            if (propertyType == null)
            {
                return null;
            }

            return FromDbModel(propertyType);
        }

        public async Task<List<PropertyTypeServiceModel>> GetAll()
        {
            return await this.db.PropertyTypes
                .OrderBy(x => x.SortRank)
                .Select(x => FromDbModel(x))
                .ToListAsync();
        }
    }
}
