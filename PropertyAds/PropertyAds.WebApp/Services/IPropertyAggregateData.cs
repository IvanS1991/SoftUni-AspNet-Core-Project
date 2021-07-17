namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using System.Threading.Tasks;

    public interface IPropertyAggregateData
    {
        Task<PropertyAggregate> Create(PropertyAggregate propertyAggregate);

        Task Populate();
    }
}
