namespace PropertyAds.WebApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static PropertyAds.WebApp.Data.DataConstants;

    public class Property
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public int Price { get; set; }

        public decimal Area { get; set; }

        public decimal UsableArea { get; set; }

        public int Floor { get; set; }

        public int TotalFloors { get; set; }

        public int Year { get; set; }

        [Required]
        [MaxLength(PropertyDescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastModifiedOn { get; set; }

        public int VisitedCount { get; set; } = 0;

        public bool IsFlagged { get; set; } = false;

        [Required]
        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public string TypeId { get; set; }

        public PropertyType Type { get; set; }

        public string DistrictId { get; set; }

        public District District { get; set; }

        public ICollection<Conversation> Conversations { get; set; } = new HashSet<Conversation>();

        public ICollection<PropertyImage> Images { get; set; } = new HashSet<PropertyImage>();
    }
}
