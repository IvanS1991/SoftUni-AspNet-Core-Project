namespace PropertyAds.WebApp.Services.WatchlistServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWatchlistData
    {
        Task<WatchlistServiceModel> Create(string name);

        Task Delete(string watchlistId);

        Task<WatchlistServiceModel> Get(string query);

        Task<List<WatchlistServiceModel>> GetAll(string userId);

        Task<List<WatchlistServiceModel>> GetForProperty(string propertyId);

        Task<List<WatchlistServiceModel>> GetForSegment(string propertyTypeId, string districtId);

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
