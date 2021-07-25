namespace PropertyAds.WebApp.Models.Property
{
    using Microsoft.AspNetCore.Http;
    using PropertyAds.WebApp.Models.District;
    using PropertyAds.WebApp.Models.PropertyType;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static PropertyAds.WebApp.Data.DataConstants;
    using static PropertyAds.WebApp.Data.DataErrors;

    public class CreatePropertyFormModel
    {
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

        [Required(
            ErrorMessage = RequiredError)]
        [Display(Name = "Вид на имота")]
        public string TypeId { get; set; }

        [Required(
            ErrorMessage = RequiredError)]
        [Display(Name = "Квартал")]
        public string DistrictId { get; set; }

        [Display(Name = "Снимки")]
        public IFormFileCollection Images { get; set; }

        public IEnumerable<PropertyTypeViewModel> Types { get; set; }

        public IEnumerable<DistrictViewModel> Districts { get; set; }
    }
}
