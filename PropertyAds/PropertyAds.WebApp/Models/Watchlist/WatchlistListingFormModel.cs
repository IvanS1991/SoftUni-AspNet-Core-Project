namespace PropertyAds.WebApp.Models.Watchlist
{
    using PropertyAds.WebApp.Services.WatchlistServices;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static PropertyAds.WebApp.Data.DataConstants;
    using static PropertyAds.WebApp.Data.DataErrors;

    public class WatchlistListingFormModel
    {

        [Required(
            ErrorMessage = RequiredError)]
        [Display(Name = "Име")]
        [StringLength(
            NameMaxLength,
            MinimumLength = NameMinLength,
            ErrorMessage = StringLengthError)]
        public string CreatedWatchlistName { get; set; }

        public IEnumerable<WatchlistServiceModel> Watchlists { get; set; }
    }
}
