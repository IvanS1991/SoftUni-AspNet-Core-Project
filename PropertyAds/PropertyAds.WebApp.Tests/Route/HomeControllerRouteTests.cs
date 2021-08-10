namespace PropertyAds.WebApp.Tests.Route
{
    using NUnit.Framework;
    using PropertyAds.WebApp.Controllers;

    using static MyTested.AspNetCore.Mvc.MyRouting;

    [TestFixture]
    public class HomeControllerRouteTests
    {
        [Test]
        public void Index_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/")
                .To<HomeController>(x => x.Index());
        }

        [Test]
        public void Error_ShouldBeMappedCorrectly()
        {
            Configuration()
                .ShouldMap("/Home/Error")
                .To<HomeController>(x => x.Error());
        }
    }
}
