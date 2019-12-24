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

    using Microsoft.AspNetCore.Hosting;
    using System.IO;
    using Microsoft.AspNetCore.Http;

    public class UserAccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IStatsUserService statsUserService;
        private readonly ITakenBooksService takenBooksService;
        private readonly IIndexUserService indexUserService;
        private readonly IUserProfileService userProfileService;
        private readonly IMessageService messageService;
        private readonly IProfileChakerService profilChekerService;

        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;
        private string userId;
        private readonly IHostingEnvironment hostingEnvironment;

        public UserAccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IIndexUserService indexUserService,
            ILogger<LogoutModel> logger,
            ITakenBooksService takenBooksService,
            IStatsUserService statsUserService,
            IUserProfileService userProfileService,
            IHostingEnvironment hostingEnvironment,
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
            this.statsUserService = statsUserService;
            this.hostingEnvironment = hostingEnvironment;
        }

        private IActionResult StartUp()
        {
            this.userId = this.UserManager.GetUserId(this.User);
            this.ViewData["UserType"] = "user";
            var chackProfile = this.profilChekerService.CheckCurrectAccount(this.userId, "user");
            if (chackProfile != null)
            {
                if (chackProfile == "admin")
                {
                    return this.RedirectToAction(nameof(AdminAccountController.Index), "AdminAccount");
                }
                else if (chackProfile == "library")
                {
                    return this.RedirectToAction(nameof(LibraryAccountController.Index), "LibraryAccount");
                }
                else if (chackProfile == "user")
                {
                    return this.RedirectToAction(nameof(UserAccountController.Index), "UserAccount");

                }
                else
                {
                    return this.LogOut();
                }
            }
            var messages = this.messageService.GetMessagesNavBar(this.userId);
            this.ViewData["MessageNavBar"] = messages;
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }
            var model = this.indexUserService.PreparedPage(this.userId);
            return this.View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult LogOut()
        {

            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult TakenBooks()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }
            var returModel = this.takenBooksService.PreparedPage(this.userId);
            return this.View(returModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult TakenBooksSearch(TakenBooksViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }
            var returModel = this.takenBooksService.TakenBooks(model, this.userId);
            return this.View("TakenBooks", returModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ChangePageTakenBooks(TakenBooksViewModel model, int id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }
            var returModel = this.takenBooksService.ChangeActivePage(model, this.userId, id);
            return this.View("TakenBooks",returModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Profile()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }
            var returModel = this.userProfileService.PreparedPage(this.userId);
            return this.View(returModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Profile(ProfilUserViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var pic = model.Photo;
            if (pic != null)
            {
                var fileName = Path.Combine(
                    this.hostingEnvironment.WebRootPath + "/img/Avatars",
                    Path.GetFileName(this.userId + "_" + pic.FileName));
                pic.CopyTo(new FileStream(fileName, FileMode.Create));
                model.AvatarLocation = "/img/Avatars/" + Path.GetFileName(fileName);
            }

            var returModel = this.userProfileService.SaveChanges(model, this.userId);
            return this.View(returModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Notification()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.messageService.GetMessagesPreparedPage(this.userId);
            return this.View(returnModel);
        }


        [Authorize]
        [HttpGet]
        public IActionResult Stats()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.statsUserService.PreparedPage(this.userId);
            this.ViewData["message"] = this.userId;
            return this.View(returnModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult StatsSearch(StatsUserViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.statsUserService.SearchStats(model, this.userId);
            this.ViewData["message"] = this.userId;
            return this.View("Stats", returnModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult NotificationChangePage(MessagesViewModel model, int id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.messageService.GetMessagesChangePage(model, this.userId, id);
            this.StartUp();

            return this.View("Notification", returnModel);
        }
    }
}
