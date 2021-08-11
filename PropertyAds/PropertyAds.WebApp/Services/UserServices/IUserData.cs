namespace PropertyAds.WebApp.Services.UserServices
{
    using System.Security.Claims;

    public interface IUserData
    {
        string GetCurrentUserId();

        bool IsAdmin();

        bool IsAdmin(ClaimsPrincipal user);
    }
}
