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
    using System.Net.Mime;
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
            return View(new PropertyFormModel
            {
                Types = await this.propertyTypeData.GetAll(),
                Districts = await this.districtData.GetAll()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] PropertyFormModel propertyModel)
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
                    UsableAreaGreaterThanAreaError);
            }

            if (!this.imageData.IsValidFormImageCollection(propertyModel.Images))
            {
                this.ModelState.AddModelError(
                    nameof(Property.Images),
                    OnlyImagesAllowedError);
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
                var hasInvalidContentType = propertyModel.Images
                    .Any(x => x.ContentType != MediaTypeNames.Image.Jpeg);

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
        public async Task<IActionResult> Edit(string id)
        {
            var property = await this.propertyData.Find(id);
            var viewModel = this.mapper.Map<PropertyFormModel>(property);

            viewModel.Id = id;
            viewModel.Types = await this.propertyTypeData.GetAll();
            viewModel.Districts = await this.districtData.GetAll();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([FromForm] PropertyFormModel propertyModel)
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
                    UsableAreaGreaterThanAreaError);
            }

            if (!this.imageData.IsValidFormImageCollection(propertyModel.Images))
            {
                this.ModelState.AddModelError(
                    nameof(Property.Images),
                    OnlyImagesAllowedError);
            }


            if (!this.ModelState.IsValid)
            {
                propertyModel.Types = await this.propertyTypeData.GetAll();
                propertyModel.Districts = await this.districtData.GetAll();

                return View(propertyModel);
            }

            await this.propertyData.Update(
               propertyModel.Id,
               propertyModel.Price,
               propertyModel.Area,
               propertyModel.UsableArea,
               propertyModel.Floor,
               propertyModel.TotalFloors,
               propertyModel.Year,
               propertyModel.Description,
               propertyModel.TypeId,
               propertyModel.DistrictId);

            return RedirectToAction(nameof(ListOwned));
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await this.propertyData.Delete(id);

            return RedirectToAction(nameof(ListOwned));
        }

        [Authorize]
        public async Task<IActionResult> List([FromQuery] PropertyListQueryModel model)
        {
            var properties = await this.propertyData.GetList(
                model.Page,
                model.DistrictId,
                model.PropertyTypeId);

            var viewModel = new PropertyListQueryModel
            {
                Districts = await this.districtData.GetAll(),
                PropertyTypes = await this.propertyTypeData.GetAll(),
                Page = model.Page,
                Rows = properties.Select(x => this.mapper.Map<PropertySummaryViewModel>(x)),
                TotalPages = await this.propertyData.TotalPageCount(
                    model.DistrictId,
                    model.PropertyTypeId)
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> ListOwned()
        {
            var properties = await this.propertyData.GetList(
                null,
                null,
                true);

            var viewModel = properties.Select(x => this.mapper.Map<PropertyDetailsViewModel>(x));

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var property = await this.propertyData
                .VisitProperty(id);

            var viewModel = this.mapper.Map<PropertyDetailsViewModel>(property);

            return View(viewModel);
        }

        public async Task<IActionResult> Image(string id)
        {
            var image = await this.imageData.GetById(id);

            return File(image.Bytes, MediaTypeNames.Image.Jpeg);
        }
    }
}
