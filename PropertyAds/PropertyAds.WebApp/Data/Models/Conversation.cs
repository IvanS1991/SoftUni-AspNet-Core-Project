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
        public string OwnerId { get; set; }

        public User Owner { get; set; }

        [Required]
        public string RecipientId { get; set; }

        public User Recipient { get; set; }

        [Required]
        public string PropertyId { get; set; }

        public Property Property { get; set; }

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
