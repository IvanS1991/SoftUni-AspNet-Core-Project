namespace PropertyAds.WebApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Watchlist
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        public DateTime LastViewedOn { get; set; }

        public ICollection<WatchlistPropertySegment> WatchlistPropertySegments { get; set; }
            = new HashSet<WatchlistPropertySegment>();

        public ICollection<WatchlistProperty> WatchlistProperties { get; set; }
            = new HashSet<WatchlistProperty>();
    }
}
