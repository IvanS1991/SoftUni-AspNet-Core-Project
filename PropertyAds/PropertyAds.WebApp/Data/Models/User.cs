namespace PropertyAds.WebApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();

        public ICollection<Conversation> OwnConversations { get; set; } = new HashSet<Conversation>();

        public ICollection<Conversation> RecipientConversations { get; set; } = new HashSet<Conversation>();
    }
}
