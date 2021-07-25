namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using System.Threading.Tasks;

    public interface IPropertyImageData
    {
        Task<PropertyImage> Create(PropertyImage image);

        Task<PropertyImage> GetById(string id);
    }
}
