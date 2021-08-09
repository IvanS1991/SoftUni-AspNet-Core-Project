namespace PropertyAds.WebApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Conversation
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public DateTime CreatedOn { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public User Author { get; set; }

        [Required]
        public string RecipientId { get; set; }

        public User Recipient { get; set; }

        public ICollection<Message> MyProperty { get; set; } = new HashSet<Message>();
    }
}
