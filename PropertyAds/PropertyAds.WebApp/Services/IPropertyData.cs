namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyData
    {
        Task<Property> Create(Property property);

        Task<Property> Find(string query);

        Task<Property> VisitProperty(string id);

        Task Update(Property property);

        Task<List<Property>> GetList();

        Task<List<Property>> GetList(int limit);

        Task<List<Property>> GetList(int limit, int offset);
    }
}
