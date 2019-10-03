namespace ELibrarySystem.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using ELibrarySystem.Data;
    using ELibrarySystem.Data.Models;
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
        private readonly IEmailSender emailSender;
        private readonly ILogger logger;

        private readonly ApplicationDbContext context;


        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.logger = logger;
            this.context = context;
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
                return this.RedirectToLocal(userId, type, returnUrl);
            }

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel indexModel, string returnUrl = null)
        {
            this.ViewBag.UserType = "guest";
            if (this.ModelState.IsValid)
            {
                var registerModel = indexModel.RegisterViewModel;
                var loginModel = indexModel.LoginViewModel;
                if (loginModel != null)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    this.ViewData["ReturnUrl"] = returnUrl;

                    return await this.Login(indexModel, returnUrl);
                }
                else
                {
                    this.ViewData["ReturnUrl"] = returnUrl;
                    return await this.Register(indexModel, returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.View(indexModel);
        }

        public async Task<IActionResult> Login(IndexViewModel indexModel, string returnUrl = null)
        {
            LoginViewModel loginModel = indexModel.LoginViewModel;

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
                    var userId = this.context.Users.FirstOrDefault(x => x.Email == loginModel.Email).Id;
                    var type = this.context.Users.FirstOrDefault(x => x.Email == loginModel.Email).Type;

                    return this.RedirectToLocal(userId, type, returnUrl);
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("User account locked out.");
                    return this.RedirectToAction(nameof(this.Lockout));
                }
            }

            this.ViewBag.LoginErr = "Невалиден Email или парола!";

            return this.View(indexModel);

        }


        public async Task<IActionResult> Register(IndexViewModel indexModel, string returnUrl = null)
        {
            this.ViewBag.UserType = "guest";
            this.ViewData["ReturnUrl"] = returnUrl;
            var registerModel = indexModel.RegisterViewModel;
            if (this.ModelState.IsValid)
            {
                var userChack = this.context.Users.FirstOrDefault(u => u.Email == registerModel.Email);
                if (userChack == null)
                {
                    var type = "user";
                  /*  var user = new ApplicationUser
                    {
                        Email = registerModel.Email,
                        UserName = registerModel.UserName,
                        Type = type,
                        Avatar = " ",
                    };*/
                    
                    var user = new ApplicationUser { 
                        UserName = registerModel.Email, 
                        Email = registerModel.Email,
                    };
                    this.ViewBag.RegisterErr = $"user.Email={user.Email} ";
                    this.ViewBag.RegisterErr += $"user.UserName={user.UserName} ";
                    this.ViewBag.RegisterErr += $"registerModel.Password={registerModel.Password} ";
                    var result = await this.userManager.CreateAsync(user, registerModel.Password);
                    //var result = await this.userManager.CreateAsync(user, registerModel.Password);
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

                        return this.RedirectToLocal(userId, type, returnUrl);
                    }
                }
                else
                {
                    this.ViewBag.RegisterErr = "Email адреса е зает!";
                }
            }

            return this.View(indexModel);
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
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string userId, string type, string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                this.HttpContext.Session.SetString("userId", userId);
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
        }
    }
}
