namespace PropertyAds.WebApp.Models.Property
{
    public class PropertySummaryViewModel : PropertyViewModelBase
    {
        public int Price { get; set; }

        public string Description { get; set; }

        public string DistrictName { get; set; }

        public string PropertyTypeName { get; set; }

        public string ImageId { get; set; }
    }
}
