namespace PropertyAds.WebApp.Services.UserServices
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using PropertyAds.WebApp.Data.Models;
    using System.Security.Claims;

    using static PropertyAds.WebApp.Data.Roles;

    public class UserData : IUserData
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        public UserData(
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public string CurrentUserId()
        {
            return this.userManager
                .GetUserId(this.httpContextAccessor.HttpContext.User);
        }

        public bool IsAdmin()
        {
            return this.IsAdmin(this.httpContextAccessor.HttpContext.User);
        }

        public bool IsAdmin(ClaimsPrincipal user)
        {
            return user.IsInRole(Administrator);
        }
    }
}
