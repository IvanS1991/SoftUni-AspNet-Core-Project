namespace PropertyAds.WebApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using PropertyAds.Scraper;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
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

        public PropertyAggregateData(
            PropertyAdsDbContext db,
            IPropertyAggregateScraper scraper,
            IPropertyTypeData propertyTypeData,
            IDistrictData districtData,
            IConfiguration config)
        {
            this.db = db;
            this.scraper = scraper;
            this.propertyTypeData = propertyTypeData;
            this.districtData = districtData;
            this.config = config;
        }

        public async Task<PropertyAggregate> Create(PropertyAggregate propertyAggregate)
        {
            var result = await this.db.PropertyAggregates
                .FirstOrDefaultAsync(x => x.DistrictId == propertyAggregate.DistrictId
                    && x.PropertyTypeId == propertyAggregate.PropertyTypeId);

            if (result == null)
            {
                result = (await this.db.PropertyAggregates.AddAsync(propertyAggregate))
                    .Entity;
            }
            else
            {
                result.AveragePrice = propertyAggregate.AveragePrice;
                result.AveragePricePerSqM = propertyAggregate.AveragePricePerSqM;
            }

            await this.db.SaveChangesAsync();

            return result;
        }

        public async Task Populate()
        {
            var result = await this.scraper.Scrape();

            foreach (var aggregate in result.Aggregates)
            {
                var propertyType = await this.propertyTypeData.Create(new PropertyType {
                    Name = aggregate.PropertyTypeName,
                    SortRank = result.PropertyTypeSortRank.GetValueOrDefault(aggregate.PropertyTypeName)
                });
                var district = await this.districtData.Create(new District { Name = aggregate.DistrictName });

                await this.Create(new PropertyAggregate
                {
                    PropertyTypeId = propertyType.Id,
                    DistrictId = district.Id,
                    AveragePrice = aggregate.AveragePrice,
                    AveragePricePerSqM = aggregate.AveragePricePerSqM
                });
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

        public Task<int> GetCount()
        {
            return this.db.PropertyAggregates
                .CountAsync();
        }

        public Task<List<PropertyAggregate>> GetAll()
        {
            return this.GetAll(0);
        }

        public Task<List<PropertyAggregate>> GetAll(int limit)
        {
            return this.GetAll(limit, 0);
        }

        public Task<List<PropertyAggregate>> GetAll(int limit, int offset)
        {
            var queryable = this.db.PropertyAggregates
                .Include(x => x.PropertyType)
                .Include(x => x.District)
                .OrderByDescending(x => x.AveragePrice)
                .ThenBy(x => x.District.Name)
                .ThenBy(x => x.PropertyType.SortRank)
                .Skip(offset);

            if (limit > 0)
            {
                queryable = queryable.Take(limit);
            }

            return queryable
                .ToListAsync();
        }

        public int GetItemsPerPage()
        {
            return this.config
                .GetSection("Pagination")
                .GetValue<int>("PropertyAggregatesList");
        }
    }
}
