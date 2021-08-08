namespace PropertyAds.WebApp.Services.WatchlistServices
{
    using System;
    using System.Collections.Generic;

    public class WatchlistServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime LastViewedOn { get; set; }

        public string OwnerId { get; set; }

        public List<WatchlistPropertySegmentServiceModel> WatchlistPropertySegments { get; set; }

        public List<WatchlistPropertyServiceModel> WatchlistProperties { get; set; }
    }
}
