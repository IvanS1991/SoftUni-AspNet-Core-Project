namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyData
    {
        Task<Property> Create(Property property);

        Task Update(Property property);

        Task<List<Property>> GetList();

        Task<List<Property>> GetList(int limit);

        Task<List<Property>> GetList(int limit, int offset);
    }
}
