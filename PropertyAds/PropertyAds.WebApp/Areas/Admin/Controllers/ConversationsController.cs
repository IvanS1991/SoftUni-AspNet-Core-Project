namespace PropertyAds.WebApp.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Services.ConversationServices;
    using System.Threading.Tasks;

    public class ConversationsController : AdminController
    {
        private readonly IConversationData conversationData;

        public ConversationsController(
            IConversationData conversationData)
        {
            this.conversationData = conversationData;
        }

        public async Task<IActionResult> Flagged()
        {
            var flaggedConversations = await this.conversationData
                .Flagged();

            return View(flaggedConversations);
        }
    }
}
