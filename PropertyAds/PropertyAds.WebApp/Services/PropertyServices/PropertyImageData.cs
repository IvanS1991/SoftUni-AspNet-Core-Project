namespace PropertyAds.WebApp.Services.PropertyServices
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using System.Threading.Tasks;

    public class PropertyImageData : IPropertyImageData
    {
        private readonly PropertyAdsDbContext db;
        private readonly IMapper mapper;

        public PropertyImageData(
            PropertyAdsDbContext db,
            IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<PropertyImageServiceModel> Create(byte[] bytes, string name, string propertyId)
        {
            var result = await this.db.PropertyImages.AddAsync(new PropertyImage {
                Bytes = bytes,
                Name = name,
                PropertyId = propertyId
            });
            await this.db.SaveChangesAsync();

            return this.mapper.Map< PropertyImageServiceModel>(result.Entity);
        }

        public async Task<PropertyImageServiceModel> GetById(string id)
        {
            var propertyImage = await this.db.PropertyImages
                .FirstOrDefaultAsync(x => x.Id == id);

            if (propertyImage == null)
            {
                return null;
            }

            return this.mapper.Map<PropertyImageServiceModel>(propertyImage);
        }
    }
}
