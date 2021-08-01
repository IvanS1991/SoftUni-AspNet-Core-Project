namespace PropertyAds.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Models.PropertyAggregate;
    using PropertyAds.WebApp.Services.DistrictServices;
    using PropertyAds.WebApp.Services.PropertyAggregateServices;
    using PropertyAds.WebApp.Services.PropertyServices;
    using System.Linq;
    using System.Threading.Tasks;

    public class PropertyAggregatesController : Controller
    {
        private readonly IPropertyAggregateData propertyAggregateData;
        private readonly IPropertyTypeData propertyTypeData;
        private readonly IDistrictData districtData;

        public PropertyAggregatesController(
            IPropertyAggregateData propertyAggregateData,
            IPropertyTypeData propertyTypeData,
            IDistrictData districtData)
        {
            this.propertyAggregateData = propertyAggregateData;
            this.propertyTypeData = propertyTypeData;
            this.districtData = districtData;
        }

        public async Task<IActionResult> List([FromQuery] PropertyAggregateListQueryModel queryModel)
        {
            var itemsPerPage = this.propertyAggregateData.GetItemsPerPage();
            var offset = (queryModel.Page - 1) * itemsPerPage;

            var aggregates = await this.propertyAggregateData.GetAll(
                itemsPerPage,
                offset,
                queryModel.DistrictId,
                queryModel.PropertyTypeId);
            var aggregatesListViewModel = new PropertyAggregateListQueryModel
            {
                Rows = aggregates.Select(
                x => new PropertyAggregateViewModel
                {
                    PropertyType = x.PropertyType.Name,
                    District = x.District.Name,
                    AveragePrice = x.AveragePrice,
                    AveragePricePerSqM = x.AveragePricePerSqM
                }),
                Page = queryModel.Page,
                TotalPages = await this.propertyAggregateData.TotalPageCount(
                    queryModel.DistrictId,
                    queryModel.PropertyTypeId),
                PropertyTypes = await this.propertyTypeData.GetAll(),
                Districts = await this.districtData.GetAll(),
                DistrictId = queryModel.DistrictId,
                PropertyTypeId = queryModel.PropertyTypeId
            };

            return View(aggregatesListViewModel);
        }
    }
}
