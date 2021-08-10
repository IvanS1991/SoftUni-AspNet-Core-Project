namespace PropertyAds.WebApp.Models.Conversation
{
    using PropertyAds.WebApp.Services.ConversationServices;
    using System.ComponentModel.DataAnnotations;

    using static PropertyAds.WebApp.Data.DataConstants;
    using static PropertyAds.WebApp.Data.DataErrors;

    public class MessageFormModel
    {
        public string PropertyId { get; set; }

        [Required]
        [StringLength(
            MessageContentMaxLength,
            MinimumLength = MessageContentMinLength,
            ErrorMessage = StringLengthError)]
        [Display(Name = "Съдържание")]
        public string Content { get; set; }

        public ConversationServiceModel Conversation { get; set; }
    }
}
