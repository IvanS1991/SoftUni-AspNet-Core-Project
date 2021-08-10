namespace PropertyAds.WebApp.Tests.Route
{
    using MyTested.AspNetCore.Mvc;
    using NUnit.Framework;
    using PropertyAds.WebApp.Controllers;
    using PropertyAds.WebApp.Models.Property;
    using static MyTested.AspNetCore.Mvc.MyRouting;

    [TestFixture]
    public class PropertiesControllerRouteTests
    {
        [Test]
        public void Create_ShouldMapCorrectly()
        {
            Configuration()
                .ShouldMap("/Properties/Create")
                .To<PropertiesController>(x => x.Create());
        }

        [Test]
        public void Create_Post_ShouldMapCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/Properties/Create")
                    .WithMethod(HttpMethod.Post))
                .To<PropertiesController>(x => x.Create(With.Any<PropertyFormModel>()));
        }

        [Test]
        public void Edit_ShouldMapCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/Properties/Edit")
                    .WithMethod(HttpMethod.Post))
                .To<PropertiesController>(x => x.Edit(With.Any<string>()));
        }

        [Test]
        public void Delete_ShouldMapCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/Properties/Delete")
                    .WithMethod(HttpMethod.Post))
                .To<PropertiesController>(x => x.Delete(With.Any<string>()));
        }

        [Test]
        public void List_ShouldMapCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/Properties/List")
                    .WithMethod(HttpMethod.Post))
                .To<PropertiesController>(x => x.List(With.Any<PropertyListQueryModel>()));
        }

        [Test]
        public void ListOwned_ShouldMapCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/Properties/ListOwned")
                    .WithMethod(HttpMethod.Post))
                .To<PropertiesController>(x => x.ListOwned());
        }

        [Test]
        public void Details_ShouldMapCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/Properties/Details")
                    .WithMethod(HttpMethod.Post))
                .To<PropertiesController>(x => x.Details(With.Any<string>()));
        }

        [Test]
        public void Image_ShouldMapCorrectly()
        {
            Configuration()
                .ShouldMap(r => r
                    .WithPath("/Properties/Image")
                    .WithMethod(HttpMethod.Post))
                .To<PropertiesController>(x => x.Image(With.Any<string>()));
        }
    }
}
