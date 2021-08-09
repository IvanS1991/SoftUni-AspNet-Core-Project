namespace PropertyAds.WebApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Models.Conversation;
    using PropertyAds.WebApp.Services.ConversationServices;
    using System.Threading.Tasks;

    [Authorize]
    public class ConversationsController : Controller
    {
        private readonly IConversationData conversationData;

        public ConversationsController(IConversationData conversationData)
        {
            this.conversationData = conversationData;
        }

        public async Task<IActionResult> Conversation(
            string id)
        {
            ConversationServiceModel conversation = null;

            if (id != null)
            {
                conversation = await this.conversationData
                    .Get(id);
            }

            var viewModel = new MessageFormModel
            {
                Conversation = conversation
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            string conversationId,
            MessageFormModel formModel)
        {
            if (conversationId == null)
            {
                var conversation = await this.conversationData
                    .Create(formModel.Content);
                conversationId = conversation.Id;
            }
            else
            {
                await this.conversationData
                    .CreateMessage(conversationId, formModel.Content);
            }

            return RedirectToAction(nameof(Conversation), new { id = conversationId });
        }
    }
}
