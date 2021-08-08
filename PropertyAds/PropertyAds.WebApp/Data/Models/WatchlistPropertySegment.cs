namespace PropertyAds.WebApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class WatchlistPropertySegment
    {
        [Required]
        public string WatchlistId { get; set; }

        public Watchlist Watchlist { get; set; }

        [Required]
        public string PropertyTypeId { get; set; }

        public PropertyType PropertyType { get; set; }

        [Required]
        public string DistrictId { get; set; }

        public District District { get; set; }
    }
}
