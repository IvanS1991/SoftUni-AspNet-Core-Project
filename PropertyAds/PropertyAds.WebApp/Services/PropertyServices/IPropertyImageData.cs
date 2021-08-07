namespace PropertyAds.WebApp.Services.PropertyServices
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IPropertyImageData
    {
        Task<PropertyImageServiceModel> Create(byte[] bytes, string name, string propertyId);

        Task Delete(string id);

        Task<PropertyImageServiceModel> GetById(string id);

        bool IsValidFormImageCollection(IFormFileCollection fileCollection);
    }
}
