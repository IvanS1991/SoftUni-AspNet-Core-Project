namespace PropertyAds.WebApp.Services.WatchlistServices
{
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyServices;

    public class WatchlistPropertySegmentServiceModel
    {
        public PropertyTypeServiceModel PropertyType { get; set; }

        public DistrictServiceModel District { get; set; }
    }
}
