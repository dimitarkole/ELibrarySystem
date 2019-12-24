namespace ELibrarySystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.AdminAccount;
    using ELibrarySystem.Services.Contracts.Home;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.Areas.Identity.Pages.Account;
    using ELibrarySystem.Web.ViewModels.AdminAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.SharedResources;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Microsoft.AspNetCore.Hosting;
    using System.IO;
    using Microsoft.AspNetCore.Http;

    public class AdminAccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;
        private string userId;

        private readonly IUsersService usersService;
        private readonly IAdminProfileService adminProfileService;
        private readonly IProfileChakerService profilChekerService;
        private readonly IMessageService messageService;
        private readonly IIndexAdminService indexAdminService;
        private readonly IStatsAdminService statsAdminService;

        private readonly IHostingEnvironment hostingEnvironment;

        public AdminAccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LogoutModel> logger,
            IUsersService usersService,
            IAdminProfileService adminProfileService,
            IProfileChakerService profilChekerService,
            IMessageService messageService,
            IIndexAdminService indexAdminService,
            IHostingEnvironment hostingEnvironment,
            IStatsAdminService statsAdminService)
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.usersService = usersService;
            this.adminProfileService = adminProfileService;
            this.profilChekerService = profilChekerService;
            this.messageService = messageService;
            this.statsAdminService = statsAdminService;
            this.indexAdminService = indexAdminService;
            this.hostingEnvironment = hostingEnvironment;
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
            var model = this.indexAdminService.PreparedPage();

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
        public IActionResult AllUsers()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.usersService.PreparedPage();
            return this.View(returnModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult MakeUserLibrary(UsersViewModel model, string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.usersService.MakeUserLibrary(model, id);
            this.ViewData["message"] = returnModel[1].ToString();
            return this.View("AllUsers", returnModel[0]);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult MakeLibraryUser(UsersViewModel model, string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.usersService.MakeLibraryUser(model, id);
            this.ViewData["message"] = returnModel[1].ToString();
            return this.View("AllUsers", returnModel[0]);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult MakeUserAdmin(UsersViewModel model, string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.usersService.MakeUserAdmin(model, id);
            this.ViewData["message"] = returnModel[1].ToString();
            return this.View("AllUsers", returnModel[0]);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult DeleteUser(UsersViewModel model, string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.usersService.MakeUserAdmin(model, id);
            this.ViewData["message"] = returnModel[1].ToString();
            return this.View("AllUsers", returnModel[0]);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangeActivePage(UsersViewModel model, int id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.usersService.ChangeActivePage(model, id);
            return this.View("AllUsers", returnModel);
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

            var returnModel = this.adminProfileService.PreparedPage(this.userId);
            return this.View(returnModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Profile(ProfilAdminViewModel model)
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

            var returnModel = this.adminProfileService.SaveChanges(model, this.userId);
            return this.View(returnModel);
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

        public IActionResult Stats()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.statsAdminService.PreparedPage(this.userId);
            this.ViewData["message"] = this.userId;
            return this.View(returnModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult StatsSearch(StatsAdminViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.statsAdminService.SearchStats(model, this.userId);
            this.ViewData["message"] = this.userId;
            return this.View("Stats", returnModel);
        }

        private IActionResult StartUp()
        {
            this.userId = this.UserManager.GetUserId(this.User);

            var chackProfile = this.profilChekerService.CheckCurrectAccount(this.userId, "admin");
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
            this.ViewData["UserType"] = "admin";
            return null;
        }
    }
}
