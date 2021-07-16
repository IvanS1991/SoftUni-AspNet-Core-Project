namespace PropertyAds.WebApp.Services
{
    using System.Threading.Tasks;

    public interface IPropertyAggregateData
    {
        Task Populate();
    }
}
