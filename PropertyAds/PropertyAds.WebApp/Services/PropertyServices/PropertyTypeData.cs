namespace PropertyAds.WebApp.Services.PropertyServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.Utility;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PropertyTypeData : IPropertyTypeData
    {
        private readonly PropertyAdsDbContext db;
        private readonly IMapper mapper;
        private readonly ICache cache;

        public PropertyTypeData(
            PropertyAdsDbContext db,
            IMapper mapper,
            ICache cache)
        {
            this.db = db;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<PropertyTypeServiceModel> Create(
            string name, int sortRank)
        {
            var existingPropertyType = await this
                .ByName(name);

            if (await this.Exists(name))
            {
                return existingPropertyType;
            }

            var result = await this.db.PropertyTypes.AddAsync(new PropertyType {
                Name = name,
                SortRank = sortRank
            });

            await this.db.SaveChangesAsync();

            return this.mapper.Map< PropertyTypeServiceModel>(result.Entity);
        }

        public Task<bool> Exists(string query)
        {
            return this.db.PropertyTypes
                .AnyAsync(x => x.Name == query || x.Id == query);
        }

        public async Task<PropertyTypeServiceModel> ByName(string propertyTypeName)
        {
            var propertyType = await this.db.PropertyTypes
                .FirstOrDefaultAsync(x => x.Name == propertyTypeName);

            if (propertyType == null)
            {
                return null;
            }

            return this.mapper.Map<PropertyTypeServiceModel>(propertyType);
        }

        public async Task<List<PropertyTypeServiceModel>> All()
        {
            List<PropertyTypeServiceModel> result;

            if (!this.cache.TryGetValue(CacheKey.PropertyTypeList, out result))
            {
                result = await this.db.PropertyTypes
                .ProjectTo<PropertyTypeServiceModel>(this.mapper.ConfigurationProvider)
                .OrderBy(x => x.SortRank)
                .ToListAsync();

                this.cache.Set(CacheKey.PropertyTypeList, result);
            }

            return result;
        }
    }
}
