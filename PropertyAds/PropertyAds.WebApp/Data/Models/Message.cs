﻿namespace PropertyAds.WebApp.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static PropertyAds.WebApp.Data.DataConstants;

    public class Message
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(
            MessageContentMaxLength)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public string ConversationId { get; set; }

        public Conversation Conversation { get; set; }
    }
}
