namespace PropertyAds.WebApp.Models.Watchlist
{
    using PropertyAds.WebApp.Models.Property;
    using System.Collections.Generic;

    public class WatchlistSegmentViewModel
    {
        public string PropertyTypeId { get; set; }

        public string DistrictId { get; set; }

        public string Name { get; set; }

        public IEnumerable<PropertyDetailsViewModel> Properties { get; set; }
    }
}
