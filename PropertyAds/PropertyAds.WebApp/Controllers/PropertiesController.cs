namespace PropertyAds.WebApp.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.Property;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Services.UserServices;
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
        private readonly IUserData userData;

        public PropertiesController(
            IPropertyData propertyData,
            IPropertyTypeData propertyTypeData,
            IDistrictData districtData,
            IPropertyImageData imageData,
            IMapper mapper,
            IUserData userData)
        {
            this.propertyData = propertyData;
            this.propertyTypeData = propertyTypeData;
            this.districtData = districtData;
            this.imageData = imageData;
            this.mapper = mapper;
            this.userData = userData;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View(new PropertyFormModel
            {
                Types = await this.propertyTypeData.All(),
                Districts = await this.districtData.All()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] PropertyFormModel propertyModel)
        {
            if (await this.propertyTypeData
                .Exists(propertyModel.TypeId) == false)
            {
                this.ModelState.AddModelError(
                    nameof(Property.TypeId),
                    PropertyTypeNotFoundError);
            }

            if (await this.districtData
                .Exists(propertyModel.DistrictId) == false)
            {
                this.ModelState.AddModelError(
                    nameof(Property.DistrictId),
                    DistrictNotFoundError);
            }

            if (propertyModel.Floor > propertyModel.TotalFloors)
            {
                this.ModelState.AddModelError(
                    nameof(Property.Floor),
                    FloorGreaterThanTotalError);
            }

            if (propertyModel.Area < propertyModel.UsableArea)
            {
                this.ModelState.AddModelError(
                    nameof(Property.UsableArea),
                    UsableAreaGreaterThanAreaError);
            }

            if (!this.imageData
                .IsValidFormImageCollection(propertyModel.Images))
            {
                this.ModelState.AddModelError(
                    nameof(Property.Images),
                    OnlyImagesAllowedError);
            }


            if (!this.ModelState.IsValid)
            {
                propertyModel.Types = await this.propertyTypeData.All();
                propertyModel.Districts = await this.districtData.All();

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

            return RedirectToAction(
                nameof(List));
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var property = await this.propertyData
                .Find(id);
            var viewModel = this.mapper
                .Map<PropertyFormModel>(property);

            viewModel.Id = id;
            viewModel.Types = await this.propertyTypeData
                .All();
            viewModel.Districts = await this.districtData
                .All();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([FromForm] PropertyFormModel propertyModel)
        {
            if (await this.propertyData
                .HasOwner(propertyModel.Id, this.userData.CurrentUserId()) == false)
            {
                return Unauthorized();
            }


            if (!this.ModelState.IsValid)
            {
                propertyModel.Types = await this.propertyTypeData.All();
                propertyModel.Districts = await this.districtData.All();

                return View(propertyModel);
            }

            await this.propertyData.Update(
               propertyModel.Id,
               propertyModel.Price,
               propertyModel.Description);

            return RedirectToAction(
                nameof(Details),
                new { id = propertyModel.Id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (await this.propertyData
                .HasOwner(id, this.userData.CurrentUserId()) == false)
            {
                return Unauthorized();
            }

            await this.propertyData
                .Delete(id);

            return RedirectToAction(
                nameof(ListOwned));
        }

        [Authorize]
        public async Task<IActionResult> List([FromQuery] PropertyListQueryModel model)
        {
            var properties = await this.propertyData.All(
                model.Page,
                model.DistrictId,
                model.PropertyTypeId);

            var viewModel = new PropertyListQueryModel
            {
                PropertyTypeId = model.PropertyTypeId,
                DistrictId = model.DistrictId,
                Districts = await this.districtData
                    .All(),
                PropertyTypes = await this.propertyTypeData
                    .All(),
                Page = model.Page,
                Rows = properties
                    .Select(x => this.mapper.Map<PropertySummaryViewModel>(x)),
                TotalPages = await this.propertyData
                    .TotalPageCount(model.DistrictId, model.PropertyTypeId)
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> ListOwned()
        {
            var properties = await this.propertyData
                .All(null, null, true);

            var viewModel = properties
                .Select(x => this.mapper.Map<PropertyDetailsViewModel>(x));

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var property = await this.propertyData
                .Visit(id);

            var viewModel = this.mapper
                .Map<PropertyDetailsViewModel>(property);

            return View(viewModel);
        }

        [ResponseCache(Duration = 1000000)]
        public async Task<IActionResult> Image(string id)
        {
            var image = await this.imageData
                .ById(id);

            return File(image.Bytes, MediaTypeNames.Image.Jpeg);
        }
    }
}
