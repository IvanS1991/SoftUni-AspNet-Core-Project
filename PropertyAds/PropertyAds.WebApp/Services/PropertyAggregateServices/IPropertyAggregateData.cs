namespace PropertyAds.WebApp.Services.PropertyAggregateServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Timers;

    public interface IPropertyAggregateData
    {
        int GetItemsPerPage();

        Task<PropertyAggregateServiceModel> Create(
            string districtId,
            string propertyTypeId,
            int averagePrice,
            int averagePricePerSqM);

        Task<int> GetCount();

        Task<List<PropertyAggregateServiceModel>> GetAll();

        Task<List<PropertyAggregateServiceModel>> GetAll(int limit);

        Task<List<PropertyAggregateServiceModel>> GetAll(int limit, int offset);

        Task Populate();

        Task<Timer> RunPopulateTask(int populateInterval);
    }
}
