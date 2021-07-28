namespace PropertyAds.WebApp.Services.PropertyServices
{
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.UserServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class PropertyData : IPropertyData
    {
        private readonly PropertyAdsDbContext db;
        private readonly IUserData userData;

        public PropertyData(
            PropertyAdsDbContext db,
            IUserData userData)
        {
            this.db = db;
            this.userData = userData;
        }

        private static PropertyServiceModel FromDbModel(Property property)
        {
            return new PropertyServiceModel
            {
                Id = property.Id,
                Price = property.Price,
                Area = property.Area,
                UsableArea = property.UsableArea,
                Floor = property.Floor,
                TotalFloors = property.TotalFloors,
                Year = property.Year,
                Description = property.Description,
                CreatedOn = property.CreatedOn,
                VisitedCount = property.VisitedCount,
                Owner = property.Owner.Email,
                Type = property.Type.Name,
                District = property.District.Name,
                ImageIds = property.Images.Select(x => x.Id)
            };
        }

        private Task<Property> FindById(string id)
        {
            return this.db.Properties
                .Include(x => x.Owner)
                .Include(x => x.Type)
                .Include(x => x.District)
                .Include(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);
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
                OwnerId = this.userData.GetCurrentUserId(),
                TypeId = typeId,
                DistrictId = districtId,
            });
            await this.db.SaveChangesAsync();

            return FromDbModel(result.Entity);
        }

        public async Task Update(Property property)
        {
            this.db.Properties.Update(property);
            await this.db.SaveChangesAsync();
        }

        public Task<List<PropertyServiceModel>> GetList()
        {
            return this.GetList(0);
        }

        public Task<List<PropertyServiceModel>> GetList(int limit)
        {
            return this.GetList(limit, 0);
        }

        public Task<List<PropertyServiceModel>> GetList(int limit, int offset)
        {
            var queryable = this.db.Properties
                .Include(x => x.Owner)
                .Include(x => x.Type)
                .Include(x => x.District)
                .Include(x => x.Images)
                .Select(x => FromDbModel(x))
                .Skip(offset);

            if (limit > 0)
            {
                queryable = queryable.Take(limit);
            }

            return queryable
                .ToListAsync();
        }

        public async Task<PropertyServiceModel> Find(string query)
        {
            var property = await this.db.Properties
                .FirstOrDefaultAsync(x => x.Id == query);

            return FromDbModel(property);
        }

        public async Task<PropertyServiceModel> VisitProperty(string id)
        {
            var property = await this.FindById(id);

            if (property == null)
            {
                return null;
            }

            property.VisitedCount += 1;

            await this.Update(property);

            return FromDbModel(property);
        }
    }
}
