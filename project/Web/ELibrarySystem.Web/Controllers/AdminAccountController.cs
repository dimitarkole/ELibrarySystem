namespace ELibrarySystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.AdminAccount;
    using ELibrarySystem.Web.Areas.Identity.Pages.Account;
    using ELibrarySystem.Web.ViewModels.AdminAccount;
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
        private string UserId;

        private IUsersService usersService;
        private IAdminProfileService adminProfileService;


        public AdminAccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LogoutModel> logger,
            IUsersService usersService,
            IAdminProfileService adminProfileService)
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.usersService = usersService;
            this.adminProfileService = adminProfileService;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            this.StarUp();
            return this.View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");

            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AllUsers()
        {
            this.StarUp();
            var returnModel = this.usersService.PreparedPage();
            return this.View(returnModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult MakeUserLibrary(UsersViewModel model, string id)
        {
            this.StarUp();
            var returnModel = this.usersService.MakeUserLibrary(model, id);
            this.ViewData["message"] = returnModel[1].ToString();
            return this.View("AllUsers", returnModel[0]);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult MakeLibraryUser(UsersViewModel model, string id)
        {
            this.StarUp();
            var returnModel = this.usersService.MakeLibraryUser(model, id);
            this.ViewData["message"] = returnModel[1].ToString();
            return this.View("AllUsers", returnModel[0]);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult MakeUserAdmin(UsersViewModel model, string id)
        {
            this.StarUp();
            var returnModel = this.usersService.MakeUserAdmin(model, id);
            this.ViewData["message"] = returnModel[1].ToString();
            return this.View("AllUsers", returnModel[0]);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult DeleteUser(UsersViewModel model, string id)
        {
            this.StarUp();
            var returnModel = this.usersService.MakeUserAdmin(model, id);
            this.ViewData["message"] = returnModel[1].ToString();
            return this.View("AllUsers", returnModel[0]);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangeActivePage(UsersViewModel model, int id)
        {
            this.StarUp();
            var returnModel = this.usersService.ChangeActivePage(model, id);
            return this.View("AllUsers", returnModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Profile()
        {
            this.StarUp();
            var returnModel = this.adminProfileService.PreparedPage(this.UserId);
            return this.View(returnModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Profile(ProfilAdminViewModel model)
        {
            this.StarUp();
            var returnModel = this.adminProfileService.SaveChanges(model, this.UserId);
            return this.View(returnModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Bar()
        {
            Random rnd = new Random();
            var chartData = new List<ChartDataViewModel>();
            chartData.Add(new ChartDataViewModel
            {
                DimensionOne = "Technology",
                Quantity = 4,
            });

            chartData.Add(new ChartDataViewModel
            {
                DimensionOne = "Sales",
                Quantity = 5,
            });

            chartData.Add(new ChartDataViewModel
            {
                DimensionOne = "Marketing",
                Quantity = 6,
            });

            chartData.Add(new ChartDataViewModel
            {
                DimensionOne = "Human Resource",
                Quantity = 7,
            });

            chartData.Add(new ChartDataViewModel
            {
                DimensionOne = "Research and Development",
                Quantity = 8,
            });

            chartData.Add(new ChartDataViewModel
            {
                DimensionOne = "Acconting",
                Quantity = 9,
            });

            chartData.Add(new ChartDataViewModel
            {
                DimensionOne = "Support",
                Quantity = 10,
            });

            chartData.Add(new ChartDataViewModel
            {
                DimensionOne = "Logistics",
                Quantity = 12,
            });

            ChartViewModel model = new ChartViewModel(chartData);

            return this.View(model);
        }

        private void StarUp()
        {
            this.UserId = this.UserManager.GetUserId(this.User);
            this.ViewBag.UserType = "admin";
        }
    }
}
