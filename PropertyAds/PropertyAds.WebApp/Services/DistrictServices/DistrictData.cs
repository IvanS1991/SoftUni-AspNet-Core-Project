namespace PropertyAds.WebApp.Services.DistrictServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DistrictData : IDistrictData
    {
        private readonly PropertyAdsDbContext db;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        public DistrictData(
            PropertyAdsDbContext db,
            IMapper mapper,
            IMemoryCache cache)
        {
            this.db = db;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<DistrictServiceModel> Create(string name)
        {
            var existingDistrict = await this.GetByName(name);

            if (await this.Exists(name))
            {
                return existingDistrict;
            }

            var result = await this.db.Districts.AddAsync(new District { Name = name });
            await this.db.SaveChangesAsync();

            return this.mapper.Map<DistrictServiceModel>(result.Entity);
        }

        public Task<bool> Exists(string query)
        {
            return this.db.Districts
                .AnyAsync(x => x.Name == query || x.Id == query);
        }

        public async Task<DistrictServiceModel> GetByName(string districtName)
        {
            var district = await this.db.Districts
                .FirstOrDefaultAsync(x => x.Name == districtName);

            if (district == null)
            {
                return null;
            }

            return this.mapper.Map<DistrictServiceModel>(district);
        }

        public async Task<List<DistrictServiceModel>> GetAll()
        {
            List<DistrictServiceModel> result;

            if (!this.cache.TryGetValue(CacheKey.DistrictList, out result))
            {
                result = await this.db.Districts
                    .ProjectTo<DistrictServiceModel>(this.mapper.ConfigurationProvider)
                    .OrderBy(x => x.Name)
                    .ToListAsync();

                this.cache.Set(CacheKey.DistrictList, result);
            }

            return result;
        }
    }
}
