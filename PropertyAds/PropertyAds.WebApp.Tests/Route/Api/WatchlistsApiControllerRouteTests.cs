namespace PropertyAds.WebApp.Tests.Route.Api
{
    using MyTested.AspNetCore.Mvc;
    using NUnit.Framework;
    using PropertyAds.WebApp.Controllers;
    using PropertyAds.WebApp.Controllers.Api;
    using PropertyAds.WebApp.Models.PropertyAggregate;

    using static MyTested.AspNetCore.Mvc.MyRouting;

    [TestFixture]
    public class WatchlistsApiControllerRouteTests
    {
        [Test]
        public void ByProperty_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/api/watchlists/by-property")
                .To<WatchlistsApiController>(x => x.ByProperty(
                    With.Any<string>()));
        }

        [Test]
        public void BySegment_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/api/watchlists/by-segment")
                .To<WatchlistsApiController>(x => x.BySegment(
                    With.Any<string>(),
                    With.Any<string>()));
        }

        [Test]
        public void AddProperty_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/api/watchlists/add-property")
                    .WithMethod(HttpMethod.Post))
                .To<WatchlistsApiController>(x => x.AddProperty(
                    With.Any<string>(),
                    With.Any<string>()));
        }

        [Test]
        public void AddSegment_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/api/watchlists/add-segment")
                    .WithMethod(HttpMethod.Post))
                .To<WatchlistsApiController>(x => x.AddSegment(
                    With.Any<string>(),
                    With.Any<string>(),
                    With.Any<string>()));
        }
    }
}
