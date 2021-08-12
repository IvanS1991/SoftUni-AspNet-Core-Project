namespace PropertyAds.WebApp.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Services.WatchlistServices;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using static PropertyAds.WebApp.Data.DataErrors;

    [ApiController]
    [Route("api/watchlists")]
    public class WatchlistsApiController : Controller
    {
        private readonly IWatchlistData watchlistData;
        private readonly IPropertyData propertyData;
        private readonly IPropertyTypeData propertyTypeData;
        private readonly IDistrictData districtData;

        public WatchlistsApiController(
            IWatchlistData watchlistData,
            IPropertyTypeData propertyTypeData,
            IDistrictData districtData,
            IPropertyData propertyData)
        {
            this.watchlistData = watchlistData;
            this.propertyTypeData = propertyTypeData;
            this.districtData = districtData;
            this.propertyData = propertyData;
        }

        [HttpGet]
        [Route("by-property")]
        [Authorize]
        public async Task<ActionResult<List<WatchlistServiceModel>>> ByProperty(
            string propertyId)
        {
            if (await this.propertyData.Exists(propertyId)
                == false)
            {
                return BadRequest(PropertyNotFoundError);
            }

            var result = await this.watchlistData.GetForProperty(propertyId);

            return result;
        }

        [HttpGet]
        [Route("by-segment")]
        [Authorize]
        public async Task<ActionResult<List<WatchlistServiceModel>>> BySegment(
            string propertyTypeId, string districtId)
        {
            if (await this.districtData.Exists(districtId) == false)
            {
                return BadRequest(DistrictNotFoundError);
            }

            if (await this.propertyTypeData.Exists(propertyTypeId) == false)
            {
                return BadRequest(PropertyTypeNotFoundError);
            }

            var result = await this.watchlistData.GetForSegment(propertyTypeId, districtId);

            return result;
        }

        [HttpPost]
        [Route("add-segment")]
        [Authorize]
        public async Task<IActionResult> AddSegment(
            string watchlistId,
            string propertyTypeId,
            string districtId)
        {
            if (await this.districtData.Exists(districtId)
                == false)
            {
                return BadRequest(DistrictNotFoundError);
            }

            if (await this.propertyTypeData.Exists(propertyTypeId)
                == false)
            {
                return BadRequest(PropertyTypeNotFoundError);
            }

            if (await this.watchlistData.Exists(watchlistId)
                == false)
            {
                return BadRequest(WatchlistNotFoundError);
            }

            await this.watchlistData.AddSegment(
                watchlistId,
                propertyTypeId,
                districtId);

            return Ok();
        }

        [HttpPost]
        [Route("add-property")]
        public async Task<IActionResult> AddProperty(
            string watchlistId,
            string propertyId)
        {
            if (await this.watchlistData.Exists(watchlistId)
                == false)
            {
                this.ModelState.AddModelError(
                    nameof(watchlistId),
                    WatchlistNotFoundError);
            }

            if (await this.propertyData.Exists(propertyId)
                == false)
            {
                this.ModelState.AddModelError(
                    nameof(propertyId),
                    PropertyNotFoundError);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await this.watchlistData.AddProperty(
                watchlistId,
                propertyId);

            return Ok();
        }
    }
}
