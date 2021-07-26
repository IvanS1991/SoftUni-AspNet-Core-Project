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
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using static PropertyAds.WebApp.Data.DataErrors;
    public class PropertiesController : Controller
    {
        private readonly IPropertyData propertyData;
        private readonly IPropertyTypeData propertyTypeData;
        private readonly IDistrictData districtData;
        private readonly IPropertyImageData imageData;
        private readonly IDataFormatter dataFormatter;

        public PropertiesController(
            IPropertyData propertyData,
            IPropertyTypeData propertyTypeData,
            IDistrictData districtData,
            IPropertyImageData imageData,
            IDataFormatter dataFormatter)
        {
            this.propertyData = propertyData;
            this.propertyTypeData = propertyTypeData;
            this.districtData = districtData;
            this.imageData = imageData;
            this.dataFormatter = dataFormatter;
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
        public async Task<IActionResult> Create([FromForm] CreatePropertyFormModel propertyModel)
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

            var property = await this.propertyData.Create(new Property
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

            if (propertyModel.Images != null && propertyModel.Images.Count() > 0)
            {
                foreach (var formImage in propertyModel.Images)
                {
                    using var memoryStream = new MemoryStream();

                    await formImage.CopyToAsync(memoryStream);

                    var image = await this.imageData.Create(new PropertyImage
                    {
                        Bytes = memoryStream.ToArray(),
                        Name = formImage.FileName,
                        PropertyId = property.Id
                    });
                }
            }

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
                    Price = this.dataFormatter.FormatCurrency(x.Price),
                    Description = x.Description,
                    PropertyTypeName = x.Type,
                    DistrictName = x.District,
                    ImageId = x.ImageIds.Count() > 0 ? x.ImageIds.First() : null
                }));
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var property = await this.propertyData
                .VisitProperty(id);

            return View(new PropertyDetailsViewModel {
                Id = property.Id,
                Price = this.dataFormatter.FormatCurrency(property.Price),
                Area = property.Area,
                UsableArea = property.UsableArea,
                Floor = property.Floor,
                TotalFloors = property.TotalFloors,
                Year = property.Year,
                Description = property.Description,
                CreatedOn = property.CreatedOn,
                VisitedCount = property.VisitedCount,
                OwnerName = property.Owner,
                Type = property.Type,
                District = property.District,
                ImageIds = property.ImageIds
            });
        }

        [Authorize]
        public async Task<IActionResult> Image(string id)
        {
            var image = await this.imageData.GetById(id);

            return File(image.Bytes, "image/jpeg");
        }
    }
}
