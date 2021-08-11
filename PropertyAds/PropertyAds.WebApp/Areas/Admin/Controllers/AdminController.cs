namespace PropertyAds.WebApp.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static PropertyAds.WebApp.Data.Roles;

    [Area("Admin")]
    [Authorize(Roles = Administrator)]
    public abstract class AdminController : Controller
    {
    }
}
