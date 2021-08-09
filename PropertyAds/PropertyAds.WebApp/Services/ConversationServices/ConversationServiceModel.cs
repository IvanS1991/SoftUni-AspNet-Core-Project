namespace PropertyAds.WebApp.Services.ConversationServices
{
    using PropertyAds.WebApp.Services.UserServices;
    using System;
    using System.Collections.Generic;

    public class ConversationServiceModel
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public UserServiceModel Owner { get; set; }

        public List<MessageServiceModel> Messages { get; set; } = new List<MessageServiceModel>();
    }
}
