namespace PropertyAds.WebApp.Services.PropertyAggregateServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Timers;

    public interface IPropertyAggregateData
    {
        int GetItemsPerPage();

        Task<int> GetCount(
            string districtId,
            string propertyTypeId);

        Task<int> TotalPageCount(string districtId, string propertyTypeId);

        Task<PropertyAggregateServiceModel> Create(
            string districtId,
            string propertyTypeId,
            int averagePrice,
            int averagePricePerSqM);

        Task<List<PropertyAggregateServiceModel>> GetAll(
            string districtId,
            string propertyTypeId);

        Task<List<PropertyAggregateServiceModel>> GetAll(
            int limit,
            string districtId,
            string propertyTypeId);

        Task<List<PropertyAggregateServiceModel>> GetAll(
            int limit,
             int offset,
            string districtId,
            string propertyTypeId);

        Task Populate();

        Task<Timer> RunPopulateTask(int populateInterval);
    }
}
