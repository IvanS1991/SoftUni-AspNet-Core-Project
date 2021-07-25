namespace PropertyAds.WebApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View(new CreatePropertyFormModel
            {
                Types = await GetPropertyTypesList(),
                Districts = await GetDistrictsList()
            });
        }

        [HttpPost]
        [Authorize]
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

            if (propertyModel.Area < propertyModel.UsableArea)
            {
                this.ModelState.AddModelError(
                    nameof(Property.UsableArea),
                    UsableAreaGreaterThanArea);
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
                UsableArea = propertyModel.UsableArea,
                Floor = propertyModel.Floor,
                TotalFloors = propertyModel.TotalFloors,
                Year = propertyModel.Year,
                Description = propertyModel.Description,
                TypeId = propertyModel.TypeId,
                DistrictId = propertyModel.DistrictId
            });

            return RedirectToAction(nameof(List));
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var properties = await this.propertyData.GetList();

            return View(properties
                .Select(x => new PropertySummaryViewModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    Description = x.Description,
                    PropertyTypeName = x.Type.Name,
                    DistrictName = x.District.Name,
                }));
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var property = await this.propertyData
                .VisitProperty(id);

            return View(new PropertyDetailsViewModel {
                Id = property.Id,
                Price = property.Price,
                Area = property.Area,
                UsableArea = property.UsableArea,
                Floor = property.Floor,
                TotalFloors = property.TotalFloors,
                Year = property.Year,
                Description = property.Description,
                CreatedOn = property.CreatedOn,
                VisitedCount = property.VisitedCount,
                OwnerName = property.Owner.Email,
                Type = property.Type.Name,
                District = property.District.Name
            });
        }
    }
}
