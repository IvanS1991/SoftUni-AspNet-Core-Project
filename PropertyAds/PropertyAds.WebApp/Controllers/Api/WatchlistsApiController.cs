namespace PropertyAds.WebApp.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Services.WatchlistServices;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Authorize]
    [Route("api/watchlists")]
    public class WatchlistsApiController : Controller
    {
        private readonly IWatchlistData watchlistData;

        public WatchlistsApiController(
            IWatchlistData watchlistData)
        {
            this.watchlistData = watchlistData;
        }

        [HttpGet]
        [Route("by-property")]
        public async Task<ActionResult<List<WatchlistServiceModel>>> ByProperty(
            string propertyId)
        {
            var result = await this.watchlistData.GetForProperty(propertyId);

            return result;
        }

        [HttpGet]
        [Route("by-segment")]
        public async Task<ActionResult<List<WatchlistServiceModel>>> BySegment(
            string propertyTypeId, string districtId)
        {
            var result = await this.watchlistData.GetForSegment(propertyTypeId, districtId);

            return result;
        }

        [HttpPost]
        [Route("add-segment")]
        public async Task<IActionResult> AddSegment(
            string watchlistId,
            string propertyTypeId,
            string districtId)
        {
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
            await this.watchlistData.AddProperty(
                watchlistId,
                propertyId);

            return Ok();
        }
    }
}
