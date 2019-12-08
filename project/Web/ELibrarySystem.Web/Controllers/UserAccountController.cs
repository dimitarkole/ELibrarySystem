namespace ELibrarySystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Services.Contracts.UserAccount;
    using ELibrarySystem.Web.Areas.Identity.Pages.Account;
    using ELibrarySystem.Web.ViewModels.UserAccount;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class UserAccountController : Controller
    {
        private IUserService userService;
        private ITakenBooksService takenBooksService;
        private IIndexUserService indexUserService;
        private IUserProfileService userProfileService;


        private SignInManager<ApplicationUser> SignInManager;
        private UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;
        private string UserId;

        public UserAccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IIndexUserService indexUserService,
            ILogger<LogoutModel> logger,
            ITakenBooksService takenBooksService,
            IUserProfileService userProfileService)
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
            this.userService = userService;
            this.signInManager = signInManager;
            this.indexUserService = indexUserService;
            this.logger = logger;
            this.takenBooksService = takenBooksService;
            this.userProfileService = userProfileService;
        }

        public void StarUp()
        {
            this.UserId = this.UserManager.GetUserId(this.User);
            this.ViewBag.UserType = "user";
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            this.StarUp();
            var model = this.indexUserService.PreparedPage(this.UserId);
            return this.View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            this.StarUp();
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");

            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult TakenBooks()
        {
            this.StarUp();
            var returModel = this.takenBooksService.PreparedPage(this.UserId);
            return this.View(returModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult TakenBooksSearch(TakenBooksViewModel model)
        {
            this.StarUp();
            var returModel = this.takenBooksService.TakenBooks(model, this.UserId);
            return this.View("TakenBooks", returModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ChangePageTakenBooks(TakenBooksViewModel model, int id)
        {
            this.StarUp();
            var returModel = this.takenBooksService.ChangeActivePage(model, this.UserId, id);
            return this.View("TakenBooks",returModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Profile()
        {
            this.StarUp();
            var returModel = this.userProfileService.PreparedPage(this.UserId);
            return this.View(returModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Profile(ProfilUserViewModel model)
        {
            this.StarUp();
            var returModel = this.userProfileService.SaveChanges(model, this.UserId);
            return this.View(returModel);
        }
    }
}
