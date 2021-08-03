namespace PropertyAds.WebApp.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.Property;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyServices;
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
        private readonly IMapper mapper;

        public PropertiesController(
            IPropertyData propertyData,
            IPropertyTypeData propertyTypeData,
            IDistrictData districtData,
            IPropertyImageData imageData,
            IMapper mapper)
        {
            this.propertyData = propertyData;
            this.propertyTypeData = propertyTypeData;
            this.districtData = districtData;
            this.imageData = imageData;
            this.mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View(new CreatePropertyFormModel
            {
                Types = await this.propertyTypeData.GetAll(),
                Districts = await this.districtData.GetAll()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreatePropertyFormModel propertyModel)
        {
            if (await this.propertyTypeData.Exists(propertyModel.TypeId)
                == false)
            {
                this.ModelState.AddModelError(
                    nameof(Property.TypeId),
                    PropertyTypeNotFoundError);
            }

            if (await this.districtData.Exists(propertyModel.DistrictId)
                == false)
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
                propertyModel.Types = await this.propertyTypeData.GetAll();
                propertyModel.Districts = await this.districtData.GetAll();

                return View(propertyModel);
            }

            var property = await this.propertyData.Create(
                propertyModel.Price,
                propertyModel.Area,
                propertyModel.UsableArea,
                propertyModel.Floor,
                propertyModel.TotalFloors,
                propertyModel.Year,
                propertyModel.Description,
                propertyModel.TypeId,
                propertyModel.DistrictId
            );

            if (propertyModel.Images != null && propertyModel.Images.Count() > 0)
            {
                foreach (var formImage in propertyModel.Images)
                {
                    using var memoryStream = new MemoryStream();

                    await formImage.CopyToAsync(memoryStream);

                    var image = await this.imageData.Create(
                        memoryStream.ToArray(),
                        formImage.FileName,
                        property.Id
                    );
                }
            }

            return RedirectToAction(nameof(List));
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
            var properties = await this.propertyData.GetList();


            return View(properties
                .Select(x => this.mapper.Map<PropertySummaryViewModel>(x)));
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var property = await this.propertyData
                .VisitProperty(id);

            var viewModel = this.mapper.Map<PropertyDetailsViewModel>(property);

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Image(string id)
        {
            var image = await this.imageData.GetById(id);

            return File(image.Bytes, "image/jpeg");
        }
    }
}
