namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyTypeData
    {
        Task<PropertyType> Create(PropertyType propertyType);

        Task<bool> Exists(string propertyTypeName);

        Task<PropertyType> GetByName(string propertyTypeName);

        Task<List<PropertyType>> GetAll();
    }
}
