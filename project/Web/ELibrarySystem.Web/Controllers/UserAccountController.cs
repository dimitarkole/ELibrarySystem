namespace ELibrarySystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.Home;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Services.Contracts.UserAccount;
    using ELibrarySystem.Web.Areas.Identity.Pages.Account;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
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
        private IMessageService messageService;
        private IProfileChakerService profilChekerService;

        private SignInManager<ApplicationUser> SignInManager;
        private UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;
        private string userId;

        public UserAccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IIndexUserService indexUserService,
            ILogger<LogoutModel> logger,
            ITakenBooksService takenBooksService,
            IUserProfileService userProfileService,
            IMessageService messageService,
            IProfileChakerService profilChekerService)
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
            this.userService = userService;
            this.signInManager = signInManager;
            this.indexUserService = indexUserService;
            this.logger = logger;
            this.takenBooksService = takenBooksService;
            this.userProfileService = userProfileService;
            this.messageService = messageService;
            this.profilChekerService = profilChekerService;
        }

        private void StarUp()
        {
            this.userId = this.UserManager.GetUserId(this.User);
            this.ViewData["UserType"] = "user";
            this.ViewData["UserId"] = this.userId;
            var chackProfile = this.profilChekerService.CheckCurrectAccount(this.userId, "user");
            if (chackProfile == false)
            {
                this.LogOut();
            }
            var messages = this.messageService.GetMessagesNavBar(this.userId);
            this.ViewData["MessageNavBar"] = messages;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            this.StarUp();
            var model = this.indexUserService.PreparedPage(this.userId);
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
            var returModel = this.takenBooksService.PreparedPage(this.userId);
            return this.View(returModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult TakenBooksSearch(TakenBooksViewModel model)
        {
            this.StarUp();
            var returModel = this.takenBooksService.TakenBooks(model, this.userId);
            return this.View("TakenBooks", returModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ChangePageTakenBooks(TakenBooksViewModel model, int id)
        {
            this.StarUp();
            var returModel = this.takenBooksService.ChangeActivePage(model, this.userId, id);
            return this.View("TakenBooks",returModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Profile()
        {
            this.StarUp();
            var returModel = this.userProfileService.PreparedPage(this.userId);
            return this.View(returModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Profile(ProfilUserViewModel model)
        {
            this.StarUp();
            var returModel = this.userProfileService.SaveChanges(model, this.userId);
            return this.View(returModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Notification()
        {
            this.StarUp();
            var returnModel = this.messageService.GetMessagesPreparedPage(this.userId);
            return this.View(returnModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult NotificationChangePage(MessagesViewModel model, int id)
        {
            this.StarUp();
            var returnModel = this.messageService.GetMessagesChangePage(model, this.userId, id);
            this.StarUp();


            return this.View("Notification", returnModel);
        }
    }
}
