namespace PropertyAds.WebApp.Models.PropertyAggregate
{
    using System.Collections.Generic;

    public class PropertyAggregateListViewModel
    {
        public int PageCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<PropertyAggregateViewModel> Rows { get; set; }
    }
}
