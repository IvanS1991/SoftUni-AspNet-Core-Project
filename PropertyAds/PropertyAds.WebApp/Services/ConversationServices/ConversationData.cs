namespace PropertyAds.WebApp.Services.ConversationServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.PropertyServices;
    using PropertyAds.WebApp.Services.UserServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ConversationData : IConversationData
    {
        private readonly PropertyAdsDbContext db;
        private readonly IUserData userData;
        private readonly IMapper mapper;
        private readonly IPropertyData propertyData;

        public ConversationData(
            PropertyAdsDbContext db,
            IUserData userData,
            IMapper mapper,
            IPropertyData propertyData)
        {
            this.db = db;
            this.userData = userData;
            this.mapper = mapper;
            this.propertyData = propertyData;
        }

        public async Task<ConversationServiceModel> Create(
            string propertyId,
            string content)
        {
            var property = await this.propertyData.Find(propertyId);
            var conversation = await this.db.Conversations.AddAsync(new Conversation
            {
                OwnerId = this.userData.GetCurrentUserId(),
                PropertyId = property.Id,
                RecipientId = property.OwnerId,
                CreatedOn = DateTime.UtcNow
            });

            await this.db.SaveChangesAsync();

            await this.CreateMessage(conversation.Entity.Id, content);

            return this.mapper.Map<ConversationServiceModel>(conversation.Entity);
        }

        public Task<ConversationServiceModel> Get(string conversationId)
        {
            return this.db.Conversations
                .ProjectTo<ConversationServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == conversationId);
        }

        public Task<List<ConversationServiceModel>> GetByParticipation(string userId)
        {
            return this.db.Conversations
                .ProjectTo<ConversationServiceModel>(this.mapper.ConfigurationProvider)
                .Where(x => x.Owner.Id == userId || x.Recipient.Id == userId)
                .ToListAsync();
        }

        public Task<List<ConversationServiceModel>> GetFlagged()
        {
            return this.db.Conversations
                .ProjectTo<ConversationServiceModel>(this.mapper.ConfigurationProvider)
                .Where(x => x.Messages.Any(m => m.IsFlagged))
                .ToListAsync();
        }

        public async Task CreateMessage(
            string conversationId,
            string content)
        {
            await this.db.Messages
                .AddAsync(new Message
                {
                    Content  = content,
                    CreatedOn  = DateTime.UtcNow,
                    AuthorId = this.userData.GetCurrentUserId(),
                    ConversationId  = conversationId
                });
            await this.db.SaveChangesAsync();
        }

        public async Task Delete(string conversationId)
        {
            var conversation = await this.db.Conversations
                .FirstOrDefaultAsync(x => x.Id == conversationId);

            foreach (var message in conversation.Messages)
            {
                this.db.Messages.Remove(message);
            }

            this.db.Conversations.Remove(conversation);

            await this.db.SaveChangesAsync();
        }

        public async Task FlagMessage(
            string messageId,
            bool isFlagged)
        {
            var message = await this.db.Messages
                .FirstOrDefaultAsync(x => x.Id == messageId);

            message.IsFlagged = isFlagged;

            this.db.Messages.Update(message);

            await this.db.SaveChangesAsync();
        }
    }
}
