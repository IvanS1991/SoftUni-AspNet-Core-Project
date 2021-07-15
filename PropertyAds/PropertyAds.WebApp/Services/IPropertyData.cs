namespace PropertyAds.WebApp.Services
{
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.Property;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyData
    {
        Task Create(Property property);

        Task Update(Property property);

        Task<List<PropertySummaryViewModel>> GetList();

        Task<List<PropertySummaryViewModel>> GetList(int limit, int offset);
    }
}
