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

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
