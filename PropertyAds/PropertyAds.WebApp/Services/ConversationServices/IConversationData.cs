namespace PropertyAds.WebApp.Services.ConversationServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IConversationData
    {
        Task<ConversationServiceModel> Create(
            string propertyId,string content);

        Task Delete(
            string conversationId);

        Task<ConversationServiceModel> Find(
            string conversationId);

        Task<List<ConversationServiceModel>> Flagged();

        Task CreateMessage(
            string conversationId, string content);

        Task FlagMessage(
            string messageId, bool isFlagged);

        Task<List<ConversationServiceModel>> ByParticipation(
            string userId);
    }
}
