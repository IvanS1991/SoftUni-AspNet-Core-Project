namespace PropertyAds.WebApp.Services.ConversationServices
{
    using PropertyAds.WebApp.Services.UserServices;
    using System;

    public class MessageServiceModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public bool IsFlagged { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ConversationId { get; set; }

        public UserServiceModel Author { get; set; }
    }
}
