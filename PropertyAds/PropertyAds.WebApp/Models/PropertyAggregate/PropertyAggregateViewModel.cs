namespace PropertyAds.WebApp.Models.PropertyAggregate
{
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyServices;

    public class PropertyAggregateViewModel
    {
        public PropertyTypeServiceModel PropertyType { get; set; }

        public DistrictServiceModel District { get; set; }

        public int AveragePrice { get; set; }

        public int AveragePricePerSqM { get; set; }
    }
}
