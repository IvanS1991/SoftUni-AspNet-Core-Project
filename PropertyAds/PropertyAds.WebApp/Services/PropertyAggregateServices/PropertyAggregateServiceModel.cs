namespace PropertyAds.WebApp.Services.PropertyAggregateServices
{
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyServices;

    public class PropertyAggregateServiceModel
    {
        public DistrictServiceModel District { get; set; }

        public PropertyTypeServiceModel PropertyType { get; set; }

        public int AveragePrice { get; set; }

        public int AveragePricePerSqM { get; set; }
    }
}
