namespace PropertyAds.WebApp.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static PropertyAds.WebApp.Data.DataConstants;

    public class Property
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public decimal Price { get; set; }

        public decimal Area { get; set; }

        public int Floor { get; set; }

        public int TotalFloors { get; set; }

        public int Year { get; set; }

        [Required]
        [MaxLength(PropertyDescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public int VisitedCount { get; set; } = 0;

        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public string TypeId { get; set; }

        public PropertyType Type { get; set; }

        public string DistrictId { get; set; }

        public District District { get; set; }
    }
}
