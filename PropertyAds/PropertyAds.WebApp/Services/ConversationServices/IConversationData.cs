namespace PropertyAds.WebApp.Services.ConversationServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IConversationData
    {
        Task<ConversationServiceModel> Create(
            string propertyId,
            string content);

        Task CreateMessage(
            string conversationId,
            string content);

        Task<ConversationServiceModel> Get(
            string conversationId);

        Task<List<ConversationServiceModel>> GetByParticipation(
            string userId);

        Task Delete(
            string conversationId);
    }
}
