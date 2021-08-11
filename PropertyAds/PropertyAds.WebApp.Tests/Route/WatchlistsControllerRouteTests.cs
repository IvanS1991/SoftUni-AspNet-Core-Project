namespace PropertyAds.WebApp.Tests.Route
{
    using MyTested.AspNetCore.Mvc;
    using NUnit.Framework;
    using PropertyAds.WebApp.Controllers;
    using PropertyAds.WebApp.Models.Watchlist;

    using static MyTested.AspNetCore.Mvc.MyRouting;

    [TestFixture]
    public class WatchlistsControllerRouteTests
    {
        [Test]
        public void List_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/Watchlists/List")
                .To<WatchlistsController>(x => x.List());
        }

        [Test]
        public void Create_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/Watchlists/Create")
                    .WithMethod(HttpMethod.Post))
                .To<WatchlistsController>(x => x
                    .Create(With.Any<WatchlistListingFormModel>()));
        }

        [Test]
        public void Delete_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/Watchlists/Delete")
                .To<WatchlistsController>(x => x.Delete(With.Any<string>()));
        }

        [Test]
        public void Details_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/Watchlists/Details")
                .To<WatchlistsController>(x => x.Details(With.Any<string>()));
        }

        [Test]
        public void RemoveProperty_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/Watchlists/RemoveProperty")
                .To<WatchlistsController>(x => x.RemoveProperty(
                    With.Any<string>(),
                    With.Any<string>()));
        }

        [Test]
        public void RemoveSegment_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/Watchlists/RemoveSegment")
                .To<WatchlistsController>(x => x.RemoveSegment(
                    With.Any<string>(),
                    With.Any<string>(),
                    With.Any<string>()));
        }
    }
}
