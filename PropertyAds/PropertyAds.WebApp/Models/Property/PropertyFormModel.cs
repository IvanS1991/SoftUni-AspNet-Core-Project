namespace PropertyAds.WebApp.Models.Property
{
    using Microsoft.AspNetCore.Http;
    using PropertyAds.WebApp.Models.Validation;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static PropertyAds.WebApp.Data.DataConstants;
    using static PropertyAds.WebApp.Data.DataErrors;

    public class PropertyFormModel
    {
        public string Id { get; set; }

        [Display(Name = "Цена")]
        [Range(
            PropertyPriceMinValue,
            PropertyPriceMaxValue,
            ErrorMessage = RangeError)]
        public int Price { get; set; }

        [Display(Name = "Площ")]
        [Range(
            PropertyAreaMinValue,
            PropertyAreaMaxValue,
            ErrorMessage = RangeError)]
        public decimal Area { get; set; }

        [Display(Name = "Използваема площ")]
        [Range(
            PropertyAreaMinValue,
            PropertyAreaMaxValue,
            ErrorMessage = RangeError)]
        public decimal UsableArea { get; set; }

        [Display(Name = "Етаж")]
        [Range(
            PropertyFloorMinValue,
            PropertyFloorMaxValue,
            ErrorMessage = RangeError)]
        public int Floor { get; set; }

        [Display(Name = "Брой етажи")]
        [Range(
            PropertyFloorMinValue,
            PropertyFloorMaxValue,
            ErrorMessage = RangeError)]
        public int TotalFloors { get; set; }

        [Display(Name = "Година на строеж")]
        [Range(
            PropertyYearMinValue,
            PropertyYearMaxValue,
            ErrorMessage = RangeError)]
        public int Year { get; set; }

        [Required(
            ErrorMessage = RequiredError)]
        [Display(Name = "Описание")]
        [StringLength(
            PropertyDescriptionMaxLength,
            MinimumLength = PropertyDescriptionMinLength,
            ErrorMessage = StringLengthError)]
        public string Description { get; set; }

        [Display(Name = "Вид на имота")]
        public string TypeId { get; set; }

        [Display(Name = "Квартал")]
        public string DistrictId { get; set; }

        [Display(Name = "Снимки")]
        [CollectionRange(0, 3,
            ErrorMessage = CollectionRangeError)]
        public IFormFileCollection Images { get; set; } = new FormFileCollection();

        public IEnumerable<PropertyTypeServiceModel> Types { get; set; }

        public IEnumerable<DistrictServiceModel> Districts { get; set; }
    }
}
