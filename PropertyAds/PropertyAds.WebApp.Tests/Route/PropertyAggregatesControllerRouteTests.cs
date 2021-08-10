namespace PropertyAds.WebApp.Tests.Route
{
    using MyTested.AspNetCore.Mvc;
    using NUnit.Framework;
    using PropertyAds.WebApp.Controllers;
    using PropertyAds.WebApp.Models.PropertyAggregate;

    using static MyTested.AspNetCore.Mvc.MyRouting;

    [TestFixture]
    public class PropertyAggregatesControllerRouteTests
    {
        [Test]
        public void List_ShouldMapCorrectly()
        {
            Configuration()
                .ShouldMap("/PropertyAggregates/List")
                .To<PropertyAggregatesController>(x => x
                    .List(With.Any<PropertyAggregateListQueryModel>()));
        }
    }
}
