namespace PropertyAds.WebApp.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public ICollection<Property> Properties { get; set; } = new HashSet<Property>();

        public ICollection<Conversation> Conversations { get; set; } = new HashSet<Conversation>();
    }
}
