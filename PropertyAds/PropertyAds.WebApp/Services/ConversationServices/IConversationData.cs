namespace PropertyAds.WebApp.Services.ConversationServices
{
    using System.Threading.Tasks;

    public interface IConversationData
    {
        Task<ConversationServiceModel> Create(
            string content);

        Task CreateMessage(
            string conversationId,
            string content);

        Task<ConversationServiceModel> Get(
            string conversationId);

        Task Delete(
            string conversationId);
    }
}
