namespace PropertyAds.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.District;
    using PropertyAds.WebApp.Models.Property;
    using PropertyAds.WebApp.Models.PropertyType;
    using PropertyAds.WebApp.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static PropertyAds.WebApp.Data.DataErrors;
    public class PropertiesController : Controller
    {
        private readonly IPropertyData propertyData;
        private readonly IPropertyTypeData propertyTypeData;
        private readonly IDistrictData districtData;

        public PropertiesController(
            IPropertyData propertyData,
            IPropertyTypeData propertyTypeData,
            IDistrictData districtData)
        {
            this.propertyData = propertyData;
            this.propertyTypeData = propertyTypeData;
            this.districtData = districtData;
        }

        private async Task<IEnumerable<PropertyTypeViewModel>> GetPropertyTypesList()
        {
            var propertyTypes = await this.propertyTypeData.GetAll();

            return propertyTypes.Select(x => new PropertyTypeViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        private async Task<IEnumerable<DistrictViewModel>> GetDistrictsList()
        {
            var districts = await this.districtData.GetAll();

            return districts.Select(x => new DistrictViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        public async Task<IActionResult> Create()
        {
            return View(new CreatePropertyFormModel
            {
                Types = await GetPropertyTypesList(),
                Districts = await GetDistrictsList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePropertyFormModel propertyModel)
        {
            var isExistingPropertyType = await this
                .propertyTypeData.Exists(propertyModel.TypeId);

            if (!isExistingPropertyType)
            {
                this.ModelState.AddModelError(
                    nameof(Property.TypeId),
                    PropertyTypeNotFoundError);
            }

            var isExistingDistrict = await this
                .districtData.Exists(propertyModel.DistrictId);

            if (!isExistingDistrict)
            {
                this.ModelState.AddModelError(
                    nameof(Property.DistrictId),
                    DistrictNotFoundError);
            }

            if (propertyModel.Floor > propertyModel.TotalFloors)
            {
                this.ModelState.AddModelError(
                    nameof(Property.DistrictId),
                    FloorGreaterThanTotalError);
            }

            if (!this.ModelState.IsValid)
            {
                propertyModel.Types = await GetPropertyTypesList();
                propertyModel.Districts = await GetDistrictsList();

                return View(propertyModel);
            }

            await this.propertyData.Create(new Property
            {
                Price = propertyModel.Price,
                Area = propertyModel.Area,
                Floor = propertyModel.Floor,
                TotalFloors = propertyModel.TotalFloors,
                Year = propertyModel.Year,
                Description = propertyModel.Description,
                TypeId = propertyModel.TypeId,
                DistrictId = propertyModel.DistrictId
            });

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> List()
        {
            var properties = await this.propertyData.GetList();

            return View(properties
                .Select(x => new PropertySummaryViewModel
                {
                    Price = x.Price,
                    Description = x.Description,
                    PropertyTypeName = x.Type.Name,
                    DistrictName = x.District.Name,
                }));
        }
    }
}
