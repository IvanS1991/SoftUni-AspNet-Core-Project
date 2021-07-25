namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.Property;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyData
    {
        Task<Property> Create(Property property);

        Task<PropertyServiceModel> Find(string query);

        Task<PropertyServiceModel> VisitProperty(string id);

        Task Update(Property property);

        Task<List<PropertyServiceModel>> GetList();

        Task<List<PropertyServiceModel>> GetList(int limit);

        Task<List<PropertyServiceModel>> GetList(int limit, int offset);
    }
}
