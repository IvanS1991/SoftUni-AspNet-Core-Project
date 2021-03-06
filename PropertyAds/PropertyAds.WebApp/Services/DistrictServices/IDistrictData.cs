namespace PropertyAds.WebApp.Services.DistrictServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDistrictData
    {
        Task<DistrictServiceModel> Create(string name);

        Task<bool> Exists(string query);

        Task<DistrictServiceModel> ByName(string districtName);

        Task<List<DistrictServiceModel>> All();
    }
}
