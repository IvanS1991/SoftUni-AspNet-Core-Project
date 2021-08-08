namespace PropertyAds.WebApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class WatchlistProperty
    {
        [Required]
        public string WatchlistId { get; set; }

        public Watchlist Watchlist { get; set; }

        [Required]
        public string PropertyId { get; set; }

        public Property Property { get; set; }
    }
}
