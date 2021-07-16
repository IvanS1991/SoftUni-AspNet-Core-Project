namespace PropertyAds.Scraper.Models
{
    public class PropertyAggregateDTO
    {
        public string DistrictName { get; set; }

        public string PropertyTypeName { get; set; }

        public int AveragePrice { get; set; }

        public int AveragePricePerSqM { get; set; }
    }
}
