namespace PropertyAds.WebApp.Tests.Mocks
{
    using Moq;
    using PropertyAds.WebApp.Services.UserServices;
    using System;

    public static class UserDataMock
    {
        public static IUserData Instance()
        {
            var mock = new Mock<IUserData>();

            mock.Setup(x => x.CurrentUserId())
                .Returns(Guid.NewGuid().ToString());

            return mock.Object;
        }
    }
}
