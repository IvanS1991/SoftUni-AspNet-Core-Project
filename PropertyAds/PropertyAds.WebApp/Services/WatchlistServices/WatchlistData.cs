using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PropertyAds.WebApp.Data;
using PropertyAds.WebApp.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PropertyAds.WebApp.Services.WatchlistServices
{
    public class WatchlistData : IWatchlistData
    {
        private PropertyAdsDbContext db;
        private IMapper mapper;

        public WatchlistData(
            PropertyAdsDbContext db,
            IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task AddProperty(string watchlistId, string propertyId)
        {
            await this.db.WatchlistProperties
                .AddAsync(new WatchlistProperty
                {
                    WatchlistId = watchlistId,
                    PropertyId = propertyId
                });

            await this.db.SaveChangesAsync();
        }

        public async Task AddSegment(string watchlistId, string propertyTypeId, string districtId)
        {
            await this.db.WatchlistPropertySegments
                .AddAsync(new WatchlistPropertySegment
                {
                    WatchlistId = watchlistId,
                    PropertyTypeId = propertyTypeId,
                    DistrictId = districtId
                });

            await this.db.SaveChangesAsync();
        }

        public async Task<WatchlistServiceModel> Create(string name)
        {
            var result = await this.db.Watchlists
                .AddAsync(new Watchlist
                {
                    Name = name
                });

            await this.db.SaveChangesAsync();

            return this.mapper.Map<WatchlistServiceModel>(result.Entity);
        }

        public Task<WatchlistServiceModel> Get(string query)
        {
            return this.db.Watchlists
                .ProjectTo<WatchlistServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == query || x.Name == query);
        }

        public Task<List<WatchlistServiceModel>> GetAll()
        {
            return this.db.Watchlists
                .ProjectTo<WatchlistServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task RemoveProperty(string watchlistId, string propertyId)
        {
            var watchlistProperty = await this.db.WatchlistProperties
                .FirstOrDefaultAsync(x => x.WatchlistId == watchlistId && x.PropertyId == propertyId);

            this.db.WatchlistProperties.Remove(watchlistProperty);

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveSegment(string watchlistId, string propertyTypeId, string districtId)
        {
            var watchlistProperty = await this.db.WatchlistPropertySegments
                .FirstOrDefaultAsync(x =>
                    x.WatchlistId == watchlistId && x.PropertyTypeId == propertyTypeId && x.DistrictId == districtId);

            this.db.WatchlistPropertySegments.Remove(watchlistProperty);

            await this.db.SaveChangesAsync();
        }
    }
}
