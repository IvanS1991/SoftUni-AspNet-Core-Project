namespace PropertyAds.WebApp.Services.WatchlistServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.UserServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class WatchlistData : IWatchlistData
    {
        private PropertyAdsDbContext db;
        private IMapper mapper;
        private IUserData userData;

        public WatchlistData(
            PropertyAdsDbContext db,
            IMapper mapper,
            IUserData userData)
        {
            this.db = db;
            this.mapper = mapper;
            this.userData = userData;
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
                    Name = name,
                    LastViewedOn = DateTime.UtcNow,
                    OwnerId = this.userData.GetCurrentUserId()
                });

            await this.db.SaveChangesAsync();

            return this.mapper.Map<WatchlistServiceModel>(result.Entity);
        }

        public async Task Delete(string watchlistId)
        {
            var watchlist = await this.db.Watchlists
                .Include(x => x.WatchlistProperties)
                .Include(x => x.WatchlistPropertySegments)
                .FirstOrDefaultAsync(x => x.Id == watchlistId);

            foreach (var watchlistProperty in watchlist.WatchlistProperties)
            {
                await this.RemoveProperty(
                    watchlistId,
                    watchlistProperty.PropertyId);
            }

            foreach (var watchlistPropertySegment in watchlist.WatchlistPropertySegments)
            {
                await this.RemoveSegment(
                    watchlistId,
                    watchlistPropertySegment.PropertyTypeId,
                    watchlistPropertySegment.DistrictId);
            }

            this.db.Watchlists
                .Remove(watchlist);

            await this.db.SaveChangesAsync();
        }

        public Task<bool> Exists(string watchlistId)
        {
            return this.db.Watchlists
                .AnyAsync(x => x.Id == watchlistId);
        }

        public Task<WatchlistServiceModel> Get(string query)
        {
            return this.db.Watchlists
                .ProjectTo<WatchlistServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == query || x.Name == query);
        }

        public Task<List<WatchlistServiceModel>> GetAll(string userId)
        {
            return this.db.Watchlists
                .ProjectTo<WatchlistServiceModel>(this.mapper.ConfigurationProvider)
                .Where(x => x.OwnerId == userId)
                .ToListAsync();
        }

        public Task<List<WatchlistServiceModel>> GetForProperty(string propertyId)
        {
            return this.db.Watchlists
                .ProjectTo<WatchlistServiceModel>(this.mapper.ConfigurationProvider)
                .Where(x => x.OwnerId == this.userData.GetCurrentUserId()
                    && !x.WatchlistProperties.Any(x => x.PropertyId == propertyId))
                .ToListAsync();
        }

        public Task<List<WatchlistServiceModel>> GetForSegment(string propertyTypeId, string districtId)
        {
            return this.db.Watchlists
                .ProjectTo<WatchlistServiceModel>(this.mapper.ConfigurationProvider)
                .Where(x => x.OwnerId == this.userData.GetCurrentUserId()
                    && !x.WatchlistPropertySegments
                        .Any(x => x.PropertyType.Id == propertyTypeId && x.District.Id == districtId))
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
