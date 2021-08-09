namespace PropertyAds.WebApp.Services.ConversationServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using PropertyAds.WebApp.Data;
    using PropertyAds.WebApp.Data.Models;
    using PropertyAds.WebApp.Services.UserServices;
    using System;
    using System.Threading.Tasks;

    public class ConversationData : IConversationData
    {
        private readonly PropertyAdsDbContext db;
        private readonly IUserData userData;
        private readonly IMapper mapper;

        public ConversationData(
            PropertyAdsDbContext db,
            IUserData userData,
            IMapper mapper)
        {
            this.db = db;
            this.userData = userData;
            this.mapper = mapper;
        }

        public async Task<ConversationServiceModel> Create(string content)
        {
            var conversation = await this.db.Conversations.AddAsync(new Conversation
            {
                OwnerId = this.userData.GetCurrentUserId(),
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
    }
}
