namespace ELibrarySystem.Services.Home
{
    using ELibrarySystem.Data;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.Home;
    using ELibrarySystem.Web.ViewModels.HomeViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class HomeService : IHomeService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly ILogger logger;

        private readonly ApplicationDbContext context;

        public HomeService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            ApplicationDbContext context)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.logger = logger;
        }

        /*public async Task<List<object>> LogInAsync(IndexViewModel indexModel)
        {
            LoginViewModel loginModel = indexModel.LoginViewModel;

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            List<object> returnResult = new List<object>();
            if (this.context.Users.FirstOrDefault(x => x.Email == loginModel.Email && x.DeletedOn == null) != null)
            {
                var userName = this.context.Users.FirstOrDefault(x => x.Email == loginModel.Email && x.DeletedOn == null).UserName;

                var result =await this.signInManager.PasswordSignInAsync(
                    userName,
                    loginModel.Password,
                    loginModel.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("Успешно влизане!");
                    var userId = this.context.Users.FirstOrDefault(x => x.Email == loginModel.Email).Id;
                    var type = this.context.Users.FirstOrDefault(x => x.Email == loginModel.Email).Type;
                    returnResult.Add(userId);
                    returnResult.Add(type);
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("Акаунта е блокиран");
                    returnResult.Add("Акаунта е блокиран");
                }
            }
            else
            {
                returnResult.Add("Невалиден Email или парола!");
            }

            return returnResult;
        }


        public void LogOut()
        {
            this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");
        }

        public void Register()
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
                    var user = new ApplicationUser
                    {
                        UserName = registerModel.Email,
                        Email = registerModel.Email,
                        Type = type,
                        Avatar = " ",
                    };
                    var result = this.userManager.CreateAsync(user, registerModel.Password);
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
            throw new NotImplementedException();
        }*/
    }
}
