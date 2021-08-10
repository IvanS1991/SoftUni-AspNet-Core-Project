namespace PropertyAds.WebApp.Tests.Route
{
    using MyTested.AspNetCore.Mvc;
    using NUnit.Framework;
    using PropertyAds.WebApp.Controllers;
    using PropertyAds.WebApp.Models.Conversation;

    using static MyTested.AspNetCore.Mvc.MyRouting;

    [TestFixture]
    public class ConversationsControllerRouteTests
    {
        [Test]
        public void Conversation_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/Conversations/Conversation/testc?propertyid=testp")
                .To<ConversationsController>(x => x.Conversation("testc", "testp"));
        }

        [Test]
        public void Create_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/Conversations/Create")
                    .WithMethod(HttpMethod.Post))
                .To<ConversationsController>(x => x.Create(
                    With.Any<string>(),
                    With.Any<string>(),
                    With.Any<MessageFormModel>()));
        }

        [Test]
        public void Delete_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/Conversations/Delete/testc")
                .To<ConversationsController>(x =>
                    x.Delete("testc"));
        }

        [Test]
        public void List_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/Conversations/List")
                .To<ConversationsController>(x => x.List());
        }
    }
}
