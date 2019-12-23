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

    public class AdminAccountController : Controller
    {
        private SignInManager<ApplicationUser> SignInManager;
        private UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;
        private string userId;

        private IUsersService usersService;
        private IAdminProfileService adminProfileService;
        private IProfileChakerService profilChekerService;
        private IMessageService messageService;
        private IIndexAdminService indexAdminService;

        public AdminAccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LogoutModel> logger,
            IUsersService usersService,
            IAdminProfileService adminProfileService,
            IProfileChakerService profilChekerService,
            IMessageService messageService,
            IIndexAdminService indexAdminService)
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.usersService = usersService;
            this.adminProfileService = adminProfileService;
            this.profilChekerService = profilChekerService;
            this.messageService = messageService;
            this.indexAdminService = indexAdminService;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            var startUp = this.StarUp();
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
            var startUp = this.StarUp();
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
            var startUp = this.StarUp();
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
            var startUp = this.StarUp();
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
            var startUp = this.StarUp();
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
            var startUp = this.StarUp();
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
            var startUp = this.StarUp();
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
            var startUp = this.StarUp();
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
            var startUp = this.StarUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.adminProfileService.SaveChanges(model, this.userId);
            return this.View(returnModel);
        }

       

        [Authorize]
        [HttpGet]
        public IActionResult Notification()
        {
            var startUp = this.StarUp();
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
            var startUp = this.StarUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.messageService.GetMessagesChangePage(model, this.userId, id);
            this.StarUp();

            return this.View("Notification", returnModel);
        }

        private IActionResult StarUp()
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
