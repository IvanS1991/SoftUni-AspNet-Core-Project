namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.District;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDistrictData
    {
        Task Create(District district);

        Task<bool> Exists(string districtName);

        Task<List<DistrictViewModel>> GetAll();
    }
}
