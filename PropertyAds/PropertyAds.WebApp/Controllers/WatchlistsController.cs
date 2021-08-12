namespace PropertyAds.WebApp.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Models.Property;
    using PropertyAds.WebApp.Models.Watchlist;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Services.UserServices;
    using PropertyAds.WebApp.Services.WatchlistServices;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    public class WatchlistsController : Controller
    {
        private readonly IWatchlistData watchlistData;
        private readonly IUserData userData;
        private readonly IPropertyData propertyData;
        private readonly IMapper mapper;

        public WatchlistsController(
            IWatchlistData watchlistData,
            IUserData userData,
            IMapper mapper, 
            IPropertyData propertyData)
        {
            this.watchlistData = watchlistData;
            this.userData = userData;
            this.mapper = mapper;
            this.propertyData = propertyData;
        }

        public async Task<IActionResult> List()
        {
            var viewModel = new WatchlistListingFormModel
            {
                Watchlists = await this.watchlistData.GetAll(this.userData.GetCurrentUserId())
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WatchlistListingFormModel formModel)
        {
            await this.watchlistData.Create(formModel.CreatedWatchlistName);

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (await this.watchlistData.HasOwner(id, this.userData.GetCurrentUserId())
                == false)
            {
                return Unauthorized();
            }

            await this.watchlistData.Delete(id);

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Details(string id)
        {
            var watchlist = await this.watchlistData
                .Get(id);
            var viewModel = this.mapper.Map<WatchlistDetailsViewModel>(watchlist);

            var viewModelProperties = await this.propertyData.GetMultipleById(
                watchlist.WatchlistProperties.Select(x => x.PropertyId));

            viewModel.Properties = viewModelProperties
                .Select(x => this.mapper.Map<PropertyDetailsViewModel>(x))
                .OrderBy(x => x.Price)
                .ToList();

            foreach (var segment in watchlist.WatchlistPropertySegments)
            {
                var segmentProperties = await this.propertyData.GetList(
                    segment.District.Id, segment.PropertyType.Id);

                viewModel.Segments.Add(new WatchlistSegmentViewModel
                {
                    DistrictId = segment.District.Id,
                    PropertyTypeId = segment.PropertyType.Id,
                    Name = $"{segment.District.Name} - {segment.PropertyType.Name}",
                    Properties = segmentProperties.Select(x => this.mapper.Map<PropertyDetailsViewModel>(x))
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> RemoveProperty(
            string watchlistId,
            string propertyId)
        {
            if (await this.watchlistData.HasOwner(watchlistId, this.userData.GetCurrentUserId())
                == false)
            {
                return Unauthorized();
            }

            await this.watchlistData.RemoveProperty(
                watchlistId, propertyId);

            return RedirectToAction(nameof(Details), new { id = watchlistId });
        }

        public async Task<IActionResult> RemoveSegment(
            string watchlistId,
            string propertyTypeId,
            string districtId)
        {
            if (await this.watchlistData.HasOwner(watchlistId, this.userData.GetCurrentUserId())
                == false)
            {
                return Unauthorized();
            }

            await this.watchlistData.RemoveSegment(
                watchlistId,
                propertyTypeId,
                districtId);

            return RedirectToAction(nameof(Details), new { id = watchlistId });
        }
    }
}
