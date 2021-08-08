namespace PropertyAds.WebApp.Services.WatchlistServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWatchlistData
    {
        Task<WatchlistServiceModel> Create(string name);

        Task<WatchlistServiceModel> Get(string query);

        Task<List<WatchlistServiceModel>> GetAll();

        Task AddProperty(
            string watchlistId,
            string propertyId);

        Task RemoveProperty(
            string watchlistId,
            string propertyId);

        Task AddSegment(
            string watchlistId,
            string propertyTypeId,
            string districtId);

        Task RemoveSegment(
            string watchlistId,
            string propertyTypeId,
            string districtId);
    }
}
