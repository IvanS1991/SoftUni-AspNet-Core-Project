namespace PropertyAds.WebApp.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PropertyImage
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public byte[] Bytes { get; set; }

        [Required]
        public string PropertyId { get; set; }

        public Property Property { get; set; }
    }
}
