namespace PropertyAds.WebApp.Models.Watchlist
{
    using PropertyAds.WebApp.Models.Property;
    using System.Collections.Generic;

    public class WatchlistDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<WatchlistSegmentViewModel> Segments { get; set; } = new List<WatchlistSegmentViewModel>();

        public List<PropertyDetailsViewModel> Properties { get; set; } = new List<PropertyDetailsViewModel>();
    }
}
