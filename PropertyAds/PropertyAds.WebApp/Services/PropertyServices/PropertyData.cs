namespace PropertyAds.WebApp.Services.PropertyServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.UserServices;
    using PropertyAds.WebApp.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;

    public class PropertyData : IPropertyData
    {
        private readonly PropertyAdsDbContext db;
        private readonly IUserData userData;
        private readonly IPropertyImageData propertyImageData;
        private readonly IMapper mapper;
        private readonly IConfiguration config;

        public PropertyData(
            PropertyAdsDbContext db,
            IUserData userData,
            IMapper mapper,
            IConfiguration config,
            IPropertyImageData propertyImageData)
        {
            this.db = db;
            this.userData = userData;
            this.mapper = mapper;
            this.config = config;
            this.propertyImageData = propertyImageData;
        }

        public async Task<PropertyServiceModel> Create(
            int price,
            decimal area,
            decimal usableArea,
            int floor,
            int totalFloors,
            int year,
            string description,
            string typeId,
            string districtId)
        {
            var result = await this.db.Properties.AddAsync(new Property
            {
                Price = price,
                Area = area,
                UsableArea = usableArea,
                Floor = floor,
                TotalFloors = totalFloors,
                Year = year,
                Description = description,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow,
                OwnerId = this.userData.GetCurrentUserId(),
                TypeId = typeId,
                DistrictId = districtId,
            });
            await this.db.SaveChangesAsync();

            return this.mapper.Map<PropertyServiceModel>(result.Entity);
        }

        public async Task Update(
            string propertyId,
            int price,
            decimal area,
            decimal usableArea,
            int floor,
            int totalFloors,
            int year,
            string description,
            string typeId,
            string districtId)
        {
            var property = await this.db.Properties.FindAsync(propertyId);

            property.Price = price;
            property.Area = area;
            property.UsableArea = usableArea;
            property.Floor = floor;
            property.TotalFloors = totalFloors;
            property.Year = year;
            property.Description = description;
            property.TypeId = typeId;
            property.DistrictId = districtId;
            property.LastModifiedOn = DateTime.UtcNow;

            this.db.Properties.Update(property);
            await this.db.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var property = await this.db.Properties.FindAsync(id);

            foreach (var image in property.Images)
            {
                await this.propertyImageData.Delete(image.Id);
            }

            this.db.Properties.Remove(property);
            await this.db.SaveChangesAsync();
        }

        public Task<List<PropertyServiceModel>> GetList(
            string districtId,
            string propertyTypeId,
            bool showOnlyOwned = false)
        {
            return this.GetList(0, districtId, propertyTypeId, showOnlyOwned);
        }

        public Task<List<PropertyServiceModel>> GetList(
            int page,
            string districtId,
            string propertyTypeId,
            bool showOnlyOwned = false)
        {
            var queryable = this.db.Properties
                .ProjectTo<PropertyServiceModel>(this.mapper.ConfigurationProvider);


            if (showOnlyOwned)
            {
                queryable = queryable.Where(x => x.OwnerId == this.userData.GetCurrentUserId());
            }

            if (!string.IsNullOrWhiteSpace(districtId) && districtId.Length > 0)
            {
                queryable = queryable.Where(x => x.District.Id == districtId);
            }

            if (!string.IsNullOrWhiteSpace(propertyTypeId) && propertyTypeId.Length > 0)
            {
                queryable = queryable.Where(x => x.Type.Id == propertyTypeId);
            }

            return queryable
                .TryApplyPagination(this.GetItemsPerPage(), page)
                .OrderBy(x => x.Price)
                .ToListAsync();
        }

        public Task<List<PropertyServiceModel>> GetLatest()
        {
            return this.db.Properties
                .ProjectTo<PropertyServiceModel>(this.mapper.ConfigurationProvider)
                .Where(x => x.ImageIds.Count() > 0)
                .OrderByDescending(x => x.CreatedOn)
                .Take(3)
                .ToListAsync();
        }

        public Task<List<PropertyServiceModel>> GetFlagged()
        {
            return this.db.Properties
                .ProjectTo<PropertyServiceModel>(this.mapper.ConfigurationProvider)
                .Where(x => x.IsFlagged)
                .ToListAsync();
        }

        public Task<PropertyServiceModel> Find(string query)
        {
            return this.db.Properties
                .ProjectTo<PropertyServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == query);
        }

        public async Task<PropertyServiceModel> VisitProperty(string id)
        {
            var property = await this.db.Properties
                .Include(x => x.Type)
                .Include(x => x.District)
                .Include(x => x.Images)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (property == null)
            {
                return null;
            }

            property.VisitedCount += 1;

            this.db.Properties.Update(property);
            await this.db.SaveChangesAsync();

            return this.mapper.Map<PropertyServiceModel>(property);
        }

        public virtual int GetItemsPerPage()
        {
            return this.config
                .GetSection("Pagination")
                .GetValue<int>("PropertyList");
        }

        public Task<int> GetCount(
            string districtId,
            string propertyTypeId)
        {
            var queryable = this.db.Properties
                .ProjectTo<PropertyServiceModel>(this.mapper.ConfigurationProvider);

            if (!string.IsNullOrWhiteSpace(districtId) && districtId.Length > 0)
            {
                queryable = queryable.Where(x => x.District.Id == districtId);
            }

            if (!string.IsNullOrWhiteSpace(propertyTypeId) && propertyTypeId.Length > 0)
            {
                queryable = queryable.Where(x => x.Type.Id == propertyTypeId);
            }

            return queryable
                .CountAsync();
        }

        public async Task<int> TotalPageCount(
            string districtId,
            string propertyTypeId)
        {
            var propertiesCount = await this
                .GetCount(districtId, propertyTypeId);
            var itemsPerPage = this
                .GetItemsPerPage();

            return (int)Math.Ceiling(propertiesCount / (float)itemsPerPage);
        }

        public Task<List<PropertyServiceModel>> GetMultipleById(IEnumerable<string> ids)
        {
            return this.db.Properties
                .ProjectTo<PropertyServiceModel>(this.mapper.ConfigurationProvider)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
        }

        public Task<bool> Exists(string propertyId)
        {
            return this.db.Properties
                .AnyAsync(x => x.Id == propertyId);
        }
    }
}
