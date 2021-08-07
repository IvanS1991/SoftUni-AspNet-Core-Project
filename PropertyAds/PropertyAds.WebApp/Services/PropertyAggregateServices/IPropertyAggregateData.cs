namespace PropertyAds.WebApp.Services.PropertyAggregateServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Timers;

    public interface IPropertyAggregateData : IPaginationList
    {
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
            int page,
            string districtId,
            string propertyTypeId);

        Task Populate();

        Task<Timer> RunPopulateTask(int populateInterval);
    }
}
