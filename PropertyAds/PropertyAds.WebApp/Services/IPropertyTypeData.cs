namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.PropertyType;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyTypeData
    {
        Task Create(PropertyType propertyType);

        Task<bool> Exists(string propertyTypeName);

        Task<List<PropertyTypeViewModel>> GetAll();
    }
}
