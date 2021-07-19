namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDistrictData
    {
        Task<District> Create(District district);

        Task<bool> Exists(string query);

        Task<District> GetByName(string districtName);

        Task<List<District>> GetAll();
    }
}
