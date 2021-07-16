namespace PropertyAds.WebApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Models.District;
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

        public async Task Create(District district)
        {
            if (await this.Exists(district.Name))
            {
                return;
            }

            await this.db.Districts.AddAsync(district);
            await this.db.SaveChangesAsync();
        }

        public Task<bool> Exists(string districtName)
        {
            return this.db.Districts
                .AnyAsync(x => x.Name == districtName);
        }

        public Task<List<DistrictViewModel>> GetAll()
        {
            return this.db.Districts
                .Select(x => new DistrictViewModel { })
                .ToListAsync();
        }
    }
}
