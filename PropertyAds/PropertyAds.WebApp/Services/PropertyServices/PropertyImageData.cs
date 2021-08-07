namespace PropertyAds.WebApp.Services.PropertyServices
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;
    using System.Threading.Tasks;

    public class PropertyImageData : IPropertyImageData
    {
        private readonly List<string> imageContentTypes = new List<string>()
        {
            MediaTypeNames.Image.Jpeg
        };

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

        public async Task Delete(string id)
        {
            var image = await this.db.PropertyImages.FindAsync(id);

            this.db.PropertyImages.Remove(image);
            await this.db.SaveChangesAsync();
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

        public bool IsValidFormImageCollection(IFormFileCollection fileCollection)
        {
            return fileCollection
                       .All(x => this.imageContentTypes.Contains(x.ContentType));
        }
    }
}
