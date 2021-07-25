namespace PropertyAds.WebApp.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static PropertyAds.WebApp.Data.DataConstants;

    public class PropertyType
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();


        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public int SortRank { get; set; }
    }
}
