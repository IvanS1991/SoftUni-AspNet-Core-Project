namespace PropertyAds.WebApp.Services
{
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using System.Threading.Tasks;

    public class PropertyImageData : IPropertyImageData
    {
        private readonly PropertyAdsDbContext db;

        public PropertyImageData(
            PropertyAdsDbContext db)
        {
            this.db = db;
        }

        public async Task<PropertyImage> Create(PropertyImage image)
        {
            var result = await this.db.PropertyImages.AddAsync(image);
            await this.db.SaveChangesAsync();

            return result.Entity;
        }

        public Task<PropertyImage> GetById(string id)
        {
            return this.db.PropertyImages
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
