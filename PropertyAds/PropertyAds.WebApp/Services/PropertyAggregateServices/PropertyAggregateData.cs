namespace PropertyAds.WebApp.Services.PropertyAggregateServices
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using PropertyAds.Scraper;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyServices;
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

        private static PropertyAggregateServiceModel FromDbModel(PropertyAggregate dbModel)
        {
            return new PropertyAggregateServiceModel
            {
                District = new DistrictServiceModel
                {
                    Id = dbModel.District.Id,
                    Name = dbModel.District.Name
                },
                PropertyType = new PropertyTypeServiceModel
                {
                    Id = dbModel.PropertyType.Id,
                    Name = dbModel.PropertyType.Name
                },
                AveragePrice = dbModel.AveragePrice,
                AveragePricePerSqM = dbModel.AveragePricePerSqM
            };
        }

        public async Task<PropertyAggregateServiceModel> Create(
            string districtId,
            string propertyTypeId,
            int averagePrice,
            int averagePricePerSqM)
        {
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

            return FromDbModel(result);
        }

        public async Task Populate()
        {
            var result = await this.scraper.Scrape();

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

        public Task<int> GetCount()
        {
            return this.db.PropertyAggregates
                .CountAsync();
        }

        public Task<List<PropertyAggregateServiceModel>> GetAll()
        {
            return this.GetAll(0);
        }

        public Task<List<PropertyAggregateServiceModel>> GetAll(int limit)
        {
            return this.GetAll(limit, 0);
        }

        public Task<List<PropertyAggregateServiceModel>> GetAll(int limit, int offset)
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
                .Select(x => FromDbModel(x))
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
