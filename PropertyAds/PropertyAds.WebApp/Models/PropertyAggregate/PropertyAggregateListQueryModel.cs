namespace PropertyAds.WebApp.Models.PropertyAggregate
{
    using PropertyAds.WebApp.Models.Property;
    using System.Collections.Generic;

    public class PropertyAggregateListQueryModel : PropertyTypeDistrictQueryModel
    {
        public IEnumerable<PropertyAggregateViewModel> Rows { get; set; }
    }
}
