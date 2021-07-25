namespace PropertyAds.WebApp.Services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using PropertyAds.WebApp.Data.Models;

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

        public string GetCurrentUserId()
        {
            return this.userManager
                .GetUserId(this.httpContextAccessor.HttpContext.User);
        }
    }
}
