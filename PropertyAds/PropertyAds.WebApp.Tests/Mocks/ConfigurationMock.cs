namespace PropertyAds.WebApp.Tests.Mocks
{
    using Microsoft.Extensions.Configuration;
    using Moq;

    public static class ConfigurationMock
    {
        private static IConfigurationSection GetSectionMock()
        {
            var mock = new Mock<IConfigurationSection>();

            return mock.Object;
        }

        public static IConfiguration Instance()
        {
            var mock = new Mock<IConfiguration>();

            mock.Setup(x => x.GetSection("Pagination"))
                .Returns(GetSectionMock());

            return mock.Object;
        }
    }
}
