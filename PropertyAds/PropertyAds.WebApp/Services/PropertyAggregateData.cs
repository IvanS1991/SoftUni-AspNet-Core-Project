namespace PropertyAds.WebApp.Services
{
    using PropertyAds.Scraper;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using System.Threading.Tasks;

    public class PropertyAggregateData : IPropertyAggregateData
    {
        private readonly PropertyAdsDbContext db;
        private readonly IPropertyAggregateScraper scraper;
        private readonly IPropertyTypeData propertyTypeData;
        private readonly IDistrictData districtData;

        public PropertyAggregateData(
            PropertyAdsDbContext db,
            IPropertyAggregateScraper scraper,
            IPropertyTypeData propertyTypeData,
            IDistrictData districtData)
        {
            this.db = db;
            this.scraper = scraper;
            this.propertyTypeData = propertyTypeData;
            this.districtData = districtData;
        }

        public async Task Populate()
        {
            var results = await this.scraper.Scrape();

            foreach (var result in results)
            {
                await this.propertyTypeData.Create(new PropertyType { Name = result.PropertyTypeName });
                await this.districtData.Create(new District { Name = result.DistrictName });
            }
        }
    }
}
