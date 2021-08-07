namespace PropertyAds.WebApp.Models.Property
{
    using System.Collections.Generic;
    
    public class PropertyListQueryModel : PropertyTypeDistrictQueryModel
    {
        public IEnumerable<PropertySummaryViewModel> Rows { get; set; }
    }
}
