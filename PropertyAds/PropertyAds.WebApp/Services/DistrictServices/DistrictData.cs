namespace PropertyAds.WebApp.Services.DistrictServices
{
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DistrictData : IDistrictData
    {
        private readonly PropertyAdsDbContext db;

        public DistrictData(PropertyAdsDbContext db)
        {
            this.db = db;
        }

        private static DistrictServiceModel FromDbModel(District dbModel)
        {
            return new DistrictServiceModel
            {
                Id = dbModel.Id,
                Name = dbModel.Name
            };
        }

        public async Task<DistrictServiceModel> Create(string name)
        {
            var existingDistrict = await this.GetByName(name);

            if (await this.Exists(name))
            {
                return existingDistrict;
            }

            var result = await this.db.Districts.AddAsync(new District { Name = name });
            await this.db.SaveChangesAsync();

            return FromDbModel(result.Entity);
        }

        public Task<bool> Exists(string query)
        {
            return this.db.Districts
                .AnyAsync(x => x.Name == query || x.Id == query);
        }

        public async Task<DistrictServiceModel> GetByName(string districtName)
        {
            var district = await this.db.Districts
                .FirstOrDefaultAsync(x => x.Name == districtName);

            if (district == null)
            {
                return null;
            }

            return FromDbModel(district);
        }

        public async Task<List<DistrictServiceModel>> GetAll()
        {
            return await this.db.Districts
                .Select(x => FromDbModel(x))
                .ToListAsync();
        }
    }
}
