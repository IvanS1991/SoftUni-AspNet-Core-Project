namespace PropertyAds.WebApp.Services.UserServices
{
    using System.Security.Claims;

    public interface IUserData
    {
        string CurrentUserId();

        bool IsAdmin();

        bool IsAdmin(ClaimsPrincipal user);
    }
}
