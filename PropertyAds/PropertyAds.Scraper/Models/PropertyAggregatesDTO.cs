using System.Collections.Generic;

namespace PropertyAds.Scraper.Models
{
    public class PropertyAggregatesDTO
    {
        public Dictionary<string, int> PropertyTypeSortRank { get; set; }
            = new Dictionary<string, int>();

        public List<PropertyAggregateDTO> Aggregates { get; set; }
            = new List<PropertyAggregateDTO>();
    }
}
