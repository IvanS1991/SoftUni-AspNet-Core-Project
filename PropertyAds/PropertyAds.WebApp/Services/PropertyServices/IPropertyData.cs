namespace PropertyAds.WebApp.Services.PropertyServices
{
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyData
    {
        Task<PropertyServiceModel> Create(
            int price,
            decimal area,
            decimal usableArea,
            int floor,
            int totalFloors,
            int year,
            string description,
            string typeId,
            string districtId);

        Task<PropertyServiceModel> Find(string query);

        Task Update(Property property);

        Task<PropertyServiceModel> VisitProperty(string id);

        Task<List<PropertyServiceModel>> GetList();

        Task<List<PropertyServiceModel>> GetList(int limit);

        Task<List<PropertyServiceModel>> GetList(int limit, int offset);
    }
}
