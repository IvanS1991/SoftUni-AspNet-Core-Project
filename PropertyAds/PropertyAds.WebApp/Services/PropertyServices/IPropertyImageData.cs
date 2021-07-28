namespace PropertyAds.WebApp.Services.PropertyServices
{
    using System.Threading.Tasks;

    public interface IPropertyImageData
    {
        Task<PropertyImageServiceModel> Create(byte[] bytes, string name, string propertyId);

        Task<PropertyImageServiceModel> GetById(string id);
    }
}
