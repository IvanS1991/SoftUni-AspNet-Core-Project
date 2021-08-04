namespace PropertyAds.WebApp.Tests.Mocks
{
    using Moq;
    using PropertyAds.Scraper;
    using PropertyAds.Scraper.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class PropertyAggregateScraperMock
    {
        public static PropertyAggregatesDTO GetMockData()
        {
            return new PropertyAggregatesDTO
            {
                PropertyTypeSortRank = new Dictionary<string, int>()
                {
                    {  "first",  1 },
                    {  "second",  2 }
                },
                Aggregates = new List<PropertyAggregateDTO>()
                {
                    new PropertyAggregateDTO
                    {
                        DistrictName = "first_district",
                        PropertyTypeName = "first",
                        AveragePrice = 250000,
                        AveragePricePerSqM = 2500
                    },
                    new PropertyAggregateDTO
                    {
                        DistrictName = "second_district",
                        PropertyTypeName = "first",
                        AveragePrice = 260000,
                        AveragePricePerSqM = 2600
                    },
                    new PropertyAggregateDTO
                    {
                        DistrictName = "first_district",
                        PropertyTypeName = "second",
                        AveragePrice = 230000,
                        AveragePricePerSqM = 2300
                    }
                }
            };
        }

        public static IPropertyAggregateScraper Instance()
        {
            var mock = new Mock<IPropertyAggregateScraper>();

            mock.Setup(x => x.Scrape())
                .Returns(Task.FromResult(GetMockData()));

            return mock.Object;
        }
    }
}
