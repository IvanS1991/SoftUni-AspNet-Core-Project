namespace PropertyAds.WebApp.Services.PropertyServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
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
        private readonly IMapper mapper;

        public PropertyData(
            PropertyAdsDbContext db,
            IUserData userData,
            IMapper mapper)
        {
            this.db = db;
            this.userData = userData;
            this.mapper = mapper;
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

            return this.mapper.Map<PropertyServiceModel>(result.Entity);
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
                .ProjectTo<PropertyServiceModel>(this.mapper.ConfigurationProvider)
                .Skip(offset);

            if (limit > 0)
            {
                queryable = queryable.Take(limit);
            }

            return queryable
                .OrderBy(x => x.Price)
                .ToListAsync();
        }

        public async Task<PropertyServiceModel> Find(string query)
        {
            var property = await this.db.Properties
                .FirstOrDefaultAsync(x => x.Id == query);

            return this.mapper.Map<PropertyServiceModel>(property);
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

            await this.Update(this.mapper.Map<Property>(property));

            return this.mapper.Map<PropertyServiceModel>(property);
        }
    }
}
