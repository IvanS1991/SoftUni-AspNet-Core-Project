namespace PropertyAds.WebApp.Services.PropertyAggregateServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using PropertyAds.Scraper;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Services.Utility;
    using PropertyAds.WebApp.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Timers;

    public class PropertyAggregateData : IPropertyAggregateData
    {
        private readonly PropertyAdsDbContext db;
        private readonly IPropertyAggregateScraper scraper;
        private readonly IPropertyTypeData propertyTypeData;
        private readonly IDistrictData districtData;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly ICache cache;

        public PropertyAggregateData(
            PropertyAdsDbContext db,
            IPropertyAggregateScraper scraper,
            IPropertyTypeData propertyTypeData,
            IDistrictData districtData,
            IConfiguration config,
            IMapper mapper,
            ICache cache)
        {
            this.db = db;
            this.scraper = scraper;
            this.propertyTypeData = propertyTypeData;
            this.districtData = districtData;
            this.config = config;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<PropertyAggregateServiceModel> Create(
            string districtId,
            string propertyTypeId,
            int averagePrice,
            int averagePricePerSqM)
        {
            var districtExists = await this.districtData.Exists(districtId);
            var propertyTypeExists = await this.propertyTypeData.Exists(propertyTypeId);

            if (!districtExists || !propertyTypeExists)
            {
                throw new ArgumentException("District or property type does not exist");
            }

            var result = await this.db.PropertyAggregates
                .FirstOrDefaultAsync(x => x.DistrictId == districtId
                    && x.PropertyTypeId == propertyTypeId);

            if (result == null)
            {
                result = (await this.db.PropertyAggregates.AddAsync(new PropertyAggregate
                    {
                        DistrictId = districtId,
                        PropertyTypeId = propertyTypeId,
                        AveragePrice = averagePrice,
                        AveragePricePerSqM = averagePricePerSqM
                    }))
                    .Entity;
            }
            else
            {
                result.AveragePrice = averagePrice;
                result.AveragePricePerSqM = averagePricePerSqM;
            }

            await this.db.SaveChangesAsync();

            return this.mapper.Map< PropertyAggregateServiceModel>(result);
        }

        public async Task Populate()
        {
            var result = await this.scraper.Scrape();

            this.cache.Remove(CacheKey.PropertyTypeList);
            this.cache.Remove(CacheKey.DistrictList);

            foreach (var aggregate in result.Aggregates)
            {
                var propertyType = await this.propertyTypeData.Create(
                    aggregate.PropertyTypeName,
                    result.PropertyTypeSortRank.GetValueOrDefault(aggregate.PropertyTypeName)
                );
                var district = await this.districtData.Create(aggregate.DistrictName);

                await this.Create(
                    district.Id,
                    propertyType.Id,
                    aggregate.AveragePrice,
                    aggregate.AveragePricePerSqM
                );
            }
        }

        public async Task<Timer> RunPopulateTask(int populateInterval)
        {
            await this.Populate();

            var timer = new Timer(populateInterval);

            timer.Elapsed += async (sender, e) => await this.Populate();
            timer.AutoReset = true;
            timer.Enabled = true;

            return timer;
        }

        public Task<List<PropertyAggregateServiceModel>> GetAll(
            int page = 0,
            string districtId = null,
            string propertyTypeId = null)
        {
            var queryable = this.db.PropertyAggregates
                .ProjectTo<PropertyAggregateServiceModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(x => x.AveragePrice)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(districtId) && districtId.Length > 0)
            {
                queryable = queryable.Where(x => x.District.Id == districtId);
            }

            if (!string.IsNullOrWhiteSpace(propertyTypeId) && propertyTypeId.Length > 0)
            {
                queryable = queryable.Where(x => x.PropertyType.Id == propertyTypeId);
            }

            return queryable
                .TryApplyPagination(this.GetItemsPerPage(), page)
                .ToListAsync();
        }

        public virtual int GetItemsPerPage()
        {
            return this.config
                .GetSection("Pagination")
                .GetValue<int>("PropertyAggregatesList");
        }

        public Task<int> GetCount(
            string districtId,
            string propertyTypeId)
        {
            var queryable = this.db.PropertyAggregates
                .ProjectTo<PropertyAggregateServiceModel>(this.mapper.ConfigurationProvider);

            if (!string.IsNullOrWhiteSpace(districtId) && districtId.Length > 0)
            {
                queryable = queryable.Where(x => x.District.Id == districtId);
            }

            if (!string.IsNullOrWhiteSpace(propertyTypeId) && propertyTypeId.Length > 0)
            {
                queryable = queryable.Where(x => x.PropertyType.Id == propertyTypeId);
            }

            return queryable
                .CountAsync();
        }

        public async Task<int> TotalPageCount(
            string districtId,
            string propertyTypeId)
        {
            var aggregatesCount = await this
                .GetCount(districtId, propertyTypeId);
            var itemsPerPage = this
                .GetItemsPerPage();

            return (int)Math.Ceiling(aggregatesCount / (float)itemsPerPage);
        }
    }
}
