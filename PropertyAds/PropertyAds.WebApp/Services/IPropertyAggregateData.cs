namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Timers;

    public interface IPropertyAggregateData
    {
        int GetItemsPerPage();

        Task<PropertyAggregate> Create(PropertyAggregate propertyAggregate);

        Task<int> GetCount();

        Task<List<PropertyAggregate>> GetAll();

        Task<List<PropertyAggregate>> GetAll(int limit);

        Task<List<PropertyAggregate>> GetAll(int limit, int offset);

        Task Populate();

        Task<Timer> RunPopulateTask(int populateInterval);
    }
}
