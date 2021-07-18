namespace PropertyAds.WebApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DistrictData : IDistrictData
    {
        private readonly PropertyAdsDbContext db;

        public DistrictData(PropertyAdsDbContext db)
        {
            this.db = db;
        }

        public async Task<District> Create(District district)
        {
            var existingDistrict = await this.GetByName(district.Name);

            if (existingDistrict != null)
            {
                return existingDistrict;
            }

            var result = await this.db.Districts.AddAsync(district);
            await this.db.SaveChangesAsync();

            return result.Entity;
        }

        public Task<bool> Exists(string districtName)
        {
            return this.db.Districts
                .AnyAsync(x => x.Name == districtName);
        }

        public Task<District> GetByName(string districtName)
        {
            return this.db.Districts
                .FirstOrDefaultAsync(x => x.Name == districtName);
        }

        public Task<List<District>> GetAll()
        {
            return this.db.Districts
                .ToListAsync();
        }
    }
}
