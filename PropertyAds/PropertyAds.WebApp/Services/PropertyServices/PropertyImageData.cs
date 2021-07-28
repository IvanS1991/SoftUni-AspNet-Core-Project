namespace PropertyAds.WebApp.Services.PropertyServices
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

        private static PropertyImageServiceModel FromDbModel(PropertyImage dbModel)
        {
            return new PropertyImageServiceModel
            {
                Id = dbModel.Id,
                Bytes = dbModel.Bytes,
                Name = dbModel.Name
            };
        }

        public async Task<PropertyImageServiceModel> Create(byte[] bytes, string name, string propertyId)
        {
            var result = await this.db.PropertyImages.AddAsync(new PropertyImage {
                Bytes = bytes,
                Name = name,
                PropertyId = propertyId
            });
            await this.db.SaveChangesAsync();

            return FromDbModel(result.Entity);
        }

        public async Task<PropertyImageServiceModel> GetById(string id)
        {
            var propertyImage = await this.db.PropertyImages
                .FirstOrDefaultAsync(x => x.Id == id);

            if (propertyImage == null)
            {
                return null;
            }

            return FromDbModel(propertyImage);
        }
    }
}
