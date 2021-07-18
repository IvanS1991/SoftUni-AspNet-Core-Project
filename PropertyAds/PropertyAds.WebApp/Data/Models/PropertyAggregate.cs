using System.ComponentModel.DataAnnotations;

namespace PropertyAds.WebApp.Data.Models
{
    public class PropertyAggregate
    {
        [Key]
        [Required]
        public string DistrictId { get; set; }

        public District District { get; set; }

        [Key]
        [Required]
        public string PropertyTypeId { get; set; }

        public PropertyType PropertyType { get; set; }

        public int AveragePrice { get; set; }

        public int AveragePricePerSqM { get; set; }
    }
}
