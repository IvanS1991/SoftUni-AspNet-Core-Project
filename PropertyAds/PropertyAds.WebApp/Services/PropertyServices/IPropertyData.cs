namespace PropertyAds.WebApp.Services.PropertyServices
{
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyData : IPaginationList
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
        Task Update(
            string propertyId,
            int price,
            string description);

        Task Delete(string id);

        Task<PropertyServiceModel> Find(string query);

        Task<bool> Exists(string propertyId);

        Task<bool> HasOwner(
            string propertyId, string userId);

        Task<PropertyServiceModel> Visit(string id);

        Task<List<PropertyServiceModel>> All(
            string districtId, string propertyTypeId, bool showOnlyOwned = false);

        Task<List<PropertyServiceModel>> All(
            int page,
            string districtId,
            string propertyTypeId,
            bool showOnlyOwned = false);

        Task<List<PropertyServiceModel>> AllById(
            IEnumerable<string> ids);

        Task<List<PropertyServiceModel>> Latest();

        Task<List<PropertyServiceModel>> Flagged();

        Task<int> Count(
            string districtId, string propertyTypeId);

        Task<int> TotalPageCount(
            string districtId, string propertyTypeId);
    }
}
