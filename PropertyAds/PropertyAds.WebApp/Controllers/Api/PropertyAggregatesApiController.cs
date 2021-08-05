namespace PropertyAds.WebApp.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Services.PropertyAggregateServices;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/property-aggregates")]
    public class PropertyAggregatesApiController : Controller
    {
        private readonly IPropertyAggregateData propertyAggregateData;

        public PropertyAggregatesApiController(
            IPropertyAggregateData propertyAggregateData)
        {
            this.propertyAggregateData = propertyAggregateData;
        }

        public async Task<ActionResult<PropertyAggregateServiceModel>> Aggregate(
            string districtId, string propertyTypeId)
        {
            var results = await this.propertyAggregateData
                .GetAll(districtId, propertyTypeId);

            if (results.Count == 0)
            {
                return NotFound();
            }

            return results.FirstOrDefault();
        }
    }
}
