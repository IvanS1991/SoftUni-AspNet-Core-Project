namespace PropertyAds.WebApp.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Models;
    using PropertyAds.WebApp.Models.Property;
    using PropertyAds.WebApp.Services.PropertyServices;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly IPropertyData propertyData;
        private readonly IMapper mapper;

        public HomeController(
            IPropertyData propertyData,
            IMapper mapper)
        {
            this.propertyData = propertyData;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await this.propertyData.GetLatest();
            var viewModel = properties.Select(x => this.mapper.Map<PropertySummaryViewModel>(x));

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
