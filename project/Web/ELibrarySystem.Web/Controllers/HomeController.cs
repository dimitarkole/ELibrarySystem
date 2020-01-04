namespace ELibrarySystem.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using ELibrarySystem.Data;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.Home;
    using ELibrarySystem.Web.Controllers;
    using ELibrarySystem.Web.ViewModels;
    using ELibrarySystem.Web.ViewModels.HomeViewModels;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ISendMail sendMail;
        private readonly ILogger logger;
        private readonly IHomeService homeService;

        private readonly ApplicationDbContext context;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ISendMail sendMail,
            ILogger<AccountController> logger,
            ApplicationDbContext context,
            IHomeService homeService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.sendMail = sendMail;
            this.logger = logger;
            this.context = context;
            this.homeService = homeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl)
        {
            if (this.ViewBag.userId != null)
            {
                var userId = this.HttpContext.Session.GetString("userId");
                var type = this.context.Users.FirstOrDefault(x => x.Id == userId).Type;
                this.ViewBag.LoginErr = " ";
                this.ViewBag.RegisterErr = " ";
                return this.RedirectToLocal(userId, type);
            }

            var model = this.homeService.GetDataForIndexPage();

            return this.View(model);
        }

        [HttpGet]
        public IActionResult LogInPage()
        {
            return this.View("LogIn");
        }

        [HttpGet]
        public IActionResult RegesterAsUser()
        {
            return this.View("RegesterAsUser");
        }

        [HttpGet]
        public IActionResult RegesterAsLibrary()
        {
            return this.View("RegesterAsLibrary");
        }

        [HttpPost]
        public async Task<IActionResult> RegesterAsUser(RegisterViewModel model)
        {
            var result = this.Register(model, "user");
            if (result != null)
            {
                return await result;
            }

            return this.View("RegesterAsUser");
        }

        [HttpPost]
        public async Task<IActionResult> RegesterAsLibrary(RegisterViewModel model)
        {
            var result = this.Register(model, "library");
            if (result != null)
            {
                return await result;
            }

            return this.View("RegesterAsLibrary");
        }

        public async Task<IActionResult> LogIn(LoginViewModel loginModel, string returnUrl = null)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            if (this.context.Users.FirstOrDefault(x => x.Email == loginModel.Email && x.DeletedOn == null) != null)
            {
                var userName = this.context.Users.FirstOrDefault(x => x.Email == loginModel.Email && x.DeletedOn == null).UserName;

                var result = await this.signInManager.PasswordSignInAsync(
                    userName,
                    loginModel.Password,
                    loginModel.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("Успешно влизане!");
                    var email = loginModel.Email;

                    var userId = this.context.Users.FirstOrDefault(x => x.Email == email).Id;
                    var type = this.context.Users.FirstOrDefault(x => x.Email == email).Type;

                    return this.RedirectToLocal(userId, type);
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("User account locked out.");
                    return this.RedirectToAction(nameof(this.Lockout));
                }
            }

            this.ViewBag.LoginErr = "Невалиден Email или парола!";

            return this.View("LogIn", loginModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");
            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return this.View();
        }

        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult VerifyEmail()
        {
            VerifyEmailViewModel model = new VerifyEmailViewModel();
            return this.View("VerifyEmail", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult VerifyEmail(VerifyEmailViewModel model)
        {
            var result = this.homeService.VerifyEmail(model);
            var verification = result["verificating"];
            var userId = this.TempData["userId"].ToString();
            if (verification == "Yes")
            {
                var type = result["type"];
                return this.RedirectToLocal(userId, type);
            }

            this.homeService.SendVerifyCodeToEmail(userId);
            return this.VerifyEmail();
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult ForgotenPasswordSendCode(ForgotenPasswordViewModel model)
        {
            var result = this.homeService.ForgotenPasswordSendCode(model);
            return this.View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string userId, string type)
        {
            this.HttpContext.Session.SetString("userId", userId);

            var email = this.context.Users.FirstOrDefault(u => u.Id == userId).Email;

            if (this.homeService.CheckVerifedEmail(userId) == false)
            {
                this.TempData["userId"] = userId;
               // this.homeService.SendVerifyCodeToEmail(userId);
                return this.VerifyEmail();
            }

            if (type == "admin")
            {
                return this.RedirectToAction(nameof(AdminAccountController.Index), "AdminAccount");
            }
            else if (type == "library")
            {
                return this.RedirectToAction(nameof(LibraryAccountController.Index), "LibraryAccount");
            }

            return this.RedirectToAction(nameof(UserAccountController.Index), "UserAccount");
        }

        private bool CheckVeryfiUser(string userEmail)
        {
            this.homeService.CheckVerifedEmail(userEmail);

            return true;
        }

        private async Task<IActionResult> Register(RegisterViewModel registerModel, string type)
        {
            if (this.ModelState.IsValid)
            {
                var userChack = this.context.Users.FirstOrDefault(u => u.Email == registerModel.Email);
                if (userChack == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = registerModel.Email,
                        Email = registerModel.Email,
                        Type = type,
                        Avatar = "/img/Avatars/defaultAvatar",
                    };
                    var result = await this.userManager.CreateAsync(user, registerModel.Password);
                    this.ViewBag.RegisterErr += $"result.Succeeded= {result.Succeeded}";

                    if (result.Succeeded)
                    {
                        this.logger.LogInformation("Успешно регистриран потребител!");
                        var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

                        // var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                        // await _emailSender.SendEmailConfirmationAsync(registerModel.Email, callbackUrl);
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        this.logger.LogInformation("Успешно регистриран потребител!");

                        var userId = user.Id;
                        Message message = new Message()
                        {
                            UserId = userId,
                            User = user,
                            TextOfMessage = "Успешно регистриран потребител!",
                        };

                        this.context.Messages.Add(message);
                        this.context.SaveChanges();

                        return this.RedirectToLocal(userId, type);
                    }
                }
                else
                {
                    this.ViewBag.RegisterErr = "Email адреса е зает!";
                }
            }

            return null;
        }
    }
}
