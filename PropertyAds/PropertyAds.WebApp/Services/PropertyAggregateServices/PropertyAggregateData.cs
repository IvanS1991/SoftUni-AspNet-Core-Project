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

        private IQueryable<PropertyAggregate> TryApplyFilter(
            IQueryable<PropertyAggregate> queryable,
            string districtId,
            string propertyTypeId)
        {
            if (!string.IsNullOrWhiteSpace(districtId) && districtId.Length > 0)
            {
                queryable = queryable.Where(x => x.DistrictId == districtId);
            }

            if (!string.IsNullOrWhiteSpace(propertyTypeId) && propertyTypeId.Length > 0)
            {
                queryable = queryable.Where(x => x.PropertyTypeId == propertyTypeId);
            }

            return queryable;
        }

        private IQueryable<PropertyAggregate> TryApplyPagination(
            IQueryable<PropertyAggregate> queryable,
            int limit,
            int offset)
        {
            if (offset > 0)
            {
                queryable = queryable.Skip(offset);
            }

            if (limit > 0)
            {
                queryable = queryable.Take(limit);
            }

            return queryable;
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

    public Task<List<PropertyAggregateServiceModel>> GetAll(
            string districtId,
            string propertyTypeId)
        {
            return this.GetAll(0, districtId, propertyTypeId);
        }

        public Task<List<PropertyAggregateServiceModel>> GetAll(
            int limit,
            string districtId,
            string propertyTypeId)
        {
            return this.GetAll(limit, 0, districtId, propertyTypeId);
        }

        public Task<List<PropertyAggregateServiceModel>> GetAll(
            int limit,
            int offset,
            string districtId,
            string propertyTypeId)
        {
            var queryable = this.db.PropertyAggregates
                .Include(x => x.PropertyType)
                .Include(x => x.District)
                .OrderByDescending(x => x.AveragePrice)
                .ThenBy(x => x.District.Name)
                .ThenBy(x => x.PropertyType.SortRank)
                .AsQueryable();

            queryable = this.TryApplyFilter(
                queryable,
                districtId,
                propertyTypeId);

            queryable = this.TryApplyPagination(
                queryable,
                limit,
                offset);

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

        public Task<int> GetCount(
            string districtId,
            string propertyTypeId)
        {
            var queryable = this.TryApplyFilter(
                this.db.PropertyAggregates,
                districtId,
                propertyTypeId);

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

            return aggregatesCount / itemsPerPage;
        }
    }
}
