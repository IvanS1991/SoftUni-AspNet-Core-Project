namespace PropertyAds.WebApp.Models.Property
{
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PropertyTypeDistrictQueryModel
    {
        [Display(Name = "Квартал")]
        public string DistrictId { get; set; }

        [Display(Name = "Вид имот")]
        public string PropertyTypeId { get; set; }

        public int Page { get; set; } = 1;

        public int TotalPages { get; set; }

        public IEnumerable<PropertyTypeServiceModel> PropertyTypes { get; set; }

        public IEnumerable<DistrictServiceModel> Districts { get; set; }
    }
}
