namespace PropertyAds.WebApp.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyAggregateServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using System.Linq;
    using System.Threading.Tasks;

    using static PropertyAds.WebApp.Data.DataErrors;

    [ApiController]
    [Route("api/property-aggregates")]
    public class PropertyAggregatesApiController : Controller
    {
        private readonly IPropertyAggregateData propertyAggregateData;
        private readonly IPropertyTypeData propertyTypeData;
        private readonly IDistrictData districtData;

        public PropertyAggregatesApiController(
            IPropertyAggregateData propertyAggregateData,
            IPropertyTypeData propertyTypeData,
            IDistrictData districtData)
        {
            this.propertyAggregateData = propertyAggregateData;
            this.propertyTypeData = propertyTypeData;
            this.districtData = districtData;
        }

        [HttpGet]
        [Route("aggregate")]
        [Authorize]
        public async Task<ActionResult<PropertyAggregateServiceModel>> Aggregate(
            string districtId, string propertyTypeId)
        {
            if (await this.districtData.Exists(districtId) == false)
            {
                return BadRequest(DistrictNotFoundError);
            }

            if (await this.propertyTypeData.Exists(propertyTypeId) == false)
            {
                return BadRequest(PropertyTypeNotFoundError);
            }

            var results = await this.propertyAggregateData
                .GetAll(0, districtId, propertyTypeId);

            if (results.Count == 0)
            {
                return NotFound();
            }

            return results.FirstOrDefault();
        }
    }
}
