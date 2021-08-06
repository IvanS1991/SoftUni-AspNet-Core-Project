namespace PropertyAds.WebApp.Tests.Mocks
{
    using Moq;
    using PropertyAds.WebApp.Services.Utility;

    public static class CacheMock
    {
        public static ICache Instance()
        {
            var mock = new Mock<ICache>();

            return mock.Object;
        }
    }
}
