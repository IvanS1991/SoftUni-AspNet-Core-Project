namespace PropertyAds.WebApp.Tests.Route.Api
{
    using MyTested.AspNetCore.Mvc;
    using NUnit.Framework;
    using PropertyAds.WebApp.Controllers;
    using PropertyAds.WebApp.Controllers.Api;
    using PropertyAds.WebApp.Models.PropertyAggregate;

    using static MyTested.AspNetCore.Mvc.MyRouting;

    [TestFixture]
    public class PropertyAggregatesApiControllerRouteTests
    {
        [Test]
        public void Aggregate_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/api/property-aggregates/aggregate")
                .To<PropertyAggregatesApiController>(x => x.Aggregate(
                    With.Any<string>(),
                    With.Any<string>()));
        }
    }
}
