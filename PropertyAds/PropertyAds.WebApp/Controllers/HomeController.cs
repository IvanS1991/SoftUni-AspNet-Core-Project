namespace PropertyAds.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models;
    using PropertyAds.WebApp.Services;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly IDistrictData districtData;
        private readonly IPropertyTypeData propertyTypeData;
        private readonly IPropertyData propertyData;

        public HomeController(IDistrictData districtData, IPropertyTypeData propertyTypeData, IPropertyData propertyData)
        {
            this.districtData = districtData;
            this.propertyTypeData = propertyTypeData;
            this.propertyData = propertyData;
        }


        public async Task<IActionResult> Index()
        {
            await this.districtData.Create(new District { Name = "test-district" });
            await this.propertyTypeData.Create(new PropertyType { Name = "test-district" });

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
