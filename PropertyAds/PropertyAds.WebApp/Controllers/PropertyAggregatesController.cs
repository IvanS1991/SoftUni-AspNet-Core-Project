namespace PropertyAds.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Models.PropertyAggregate;
    using PropertyAds.WebApp.Services.PropertyAggregateServices;
    using System.Linq;
    using System.Threading.Tasks;

    public class PropertyAggregatesController : Controller
    {
        private readonly IPropertyAggregateData propertyAggregateData;

        public PropertyAggregatesController(
            IPropertyAggregateData propertyAggregateData)
        {
            this.propertyAggregateData = propertyAggregateData;
        }

        public async Task<IActionResult> List(int page = 1)
        {
            var itemsPerPage = this.propertyAggregateData.GetItemsPerPage();
            var offset = (page - 1) * itemsPerPage;

            var aggregates = await this.propertyAggregateData.GetAll(itemsPerPage, offset);
            var aggregatesListViewModel = new PropertyAggregateListViewModel
            {
                Rows = aggregates.Select(
                x => new PropertyAggregateViewModel
                {
                    PropertyType = x.PropertyType.Name,
                    District = x.District.Name,
                    AveragePrice = x.AveragePrice,
                    AveragePricePerSqM = x.AveragePricePerSqM
                }),
                PageCount = await PageCount(),
                CurrentPage = page
            };

            return View(aggregatesListViewModel);
        }

        public async Task<int> PageCount()
        {
            var aggregatesCount = await this.propertyAggregateData
                .GetCount();
            var itemsPerPage = this.propertyAggregateData
                .GetItemsPerPage();

            return aggregatesCount / itemsPerPage;
        }
    }
}
