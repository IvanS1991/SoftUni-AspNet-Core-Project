﻿namespace PropertyAds.WebApp.Services.PropertyServices
{
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyData : IPaginationList
    {
        Task<PropertyServiceModel> Create(
            int price,
            decimal area,
            decimal usableArea,
            int floor,
            int totalFloors,
            int year,
            string description,
            string typeId,
            string districtId);

        Task<PropertyServiceModel> Find(string query);

        Task Update(Property property);

        Task<PropertyServiceModel> VisitProperty(string id);

        Task<List<PropertyServiceModel>> GetList(
            string districtId,
            string propertyTypeId,
            bool showOnlyOwned = false);

        Task<List<PropertyServiceModel>> GetList(
            int page,
            string districtId,
            string propertyTypeId,
            bool showOnlyOwned = false);
        Task<int> GetCount(
            string districtId,
            string propertyTypeId);

        Task<int> TotalPageCount(string districtId, string propertyTypeId);
    }
}
