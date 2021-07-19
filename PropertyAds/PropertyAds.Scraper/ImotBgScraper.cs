namespace PropertyAds.Scraper
{
    using PropertyAds.Scraper.Core;
    using PropertyAds.Scraper.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ImotBgScraper : HTMLScraper<PropertyAggregatesDTO>, IPropertyAggregateScraper
    {
        private readonly string URL = "https://www.imot.bg/pcgi/imot.cgi?act=14";
        private HTMLDocument document;

        protected override Task Cleanup()
        {
            return Task.FromResult<object>(null);
        }

        protected override async Task<PropertyAggregatesDTO> GetResults()
        {
            var propertyTypeElements = (await this.document
                .FindAllAsync(x => x.ByQuery(".tabStatHead")))
                    .Take(3)
                    .ToArray();
            var tableRows = (await this.document
                .FindAllAsync(x => x.ByQuery(".tabStat tr")))
                    .Skip(2);

            PropertyAggregatesDTO result = new PropertyAggregatesDTO();

            for (int i = 0; i < propertyTypeElements.Length; i++)
            {
                result.PropertyTypeSortRank.Add(propertyTypeElements[i].InnerText, i);
            }

            foreach (var row in tableRows)
            {
                var cells = await row.FindAllAsync(x => x.ByQuery("td b"));

                if (cells == null || cells.Count == 0)
                {
                    continue;
                }

                var cellsAsArray = cells.ToArray();

                for (int i = 0; i < propertyTypeElements.Length; i++)
                {
                    var type = propertyTypeElements[i];

                    var propertyAggregate = new PropertyAggregateDTO
                    {
                        PropertyTypeName = type.InnerText,
                        DistrictName = cells.First().InnerText
                    };

                    if (i < cellsAsArray.Length - 1)
                    {
                        string propertyAvgPriceText = string.Join(
                            "",
                            cellsAsArray[i + 1]
                                .InnerText
                                .Split(" ", StringSplitOptions.RemoveEmptyEntries));
                        int propertyAvgPrice;

                        if (int.TryParse(propertyAvgPriceText, out propertyAvgPrice))
                        {
                            propertyAggregate.AveragePrice = propertyAvgPrice;
                        }
                    }

                    result.Aggregates.Add(propertyAggregate);
                }
            }

            return result;
        }

        protected override async Task NavigateTo()
        {
            this.document = await HTMLDocument.LoadFromUrlAsync(this.URL);
        }

        protected override Task Setup()
        {
            return Task.FromResult<object>(null);
        }
    }
}
