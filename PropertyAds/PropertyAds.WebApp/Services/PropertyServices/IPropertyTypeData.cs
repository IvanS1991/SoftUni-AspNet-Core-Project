namespace PropertyAds.WebApp.Services.PropertyServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyTypeData
    {
        Task<PropertyTypeServiceModel> Create(
            string name, int sortRank);

        Task<bool> Exists(string query);

        Task<PropertyTypeServiceModel> ByName(string propertyTypeName);

        Task<List<PropertyTypeServiceModel>> All();
    }
}
