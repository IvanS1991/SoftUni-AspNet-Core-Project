namespace PropertyAds.WebApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PropertyAds.WebApp.Models.Conversation;
    using PropertyAds.WebApp.Services.ConversationServices;
    using PropertyAds.WebApp.Services.UserServices;
    using System.Threading.Tasks;

    [Authorize]
    public class ConversationsController : Controller
    {
        private readonly IConversationData conversationData;
        private readonly IUserData userData;

        public ConversationsController(
            IConversationData conversationData,
            IUserData userData)
        {
            this.conversationData = conversationData;
            this.userData = userData;
        }

        public async Task<IActionResult> Conversation(
            string id,
            string propertyId)
        {
            ConversationServiceModel conversation = null;

            if (id != null)
            {
                conversation = await this.conversationData
                    .Get(id);
            }

            var viewModel = new MessageFormModel
            {
                PropertyId = propertyId,
                Conversation = conversation
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            string conversationId,
            string propertyId,
            MessageFormModel formModel)
        {
            if (conversationId == null)
            {
                var conversation = await this.conversationData
                    .Create(propertyId, formModel.Content);
                conversationId = conversation.Id;
            }
            else
            {
                await this.conversationData
                    .CreateMessage(conversationId, formModel.Content);
            }

            return RedirectToAction(nameof(Conversation), new { id = conversationId });
        }

        public async Task<IActionResult> Delete(
            string id)
        {
            await this.conversationData.Delete(id);

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> List()
        {
            var conversations = await this.conversationData
                .GetByParticipation(this.userData.GetCurrentUserId());

            return View(conversations);
        }

        public async Task<IActionResult> FlagMessage(
            string conversationId,
            string messageId)
        {
            await this.conversationData.FlagMessage(messageId, true);

            return RedirectToAction(nameof(Conversation), new { id = conversationId });
        }
    }
}
