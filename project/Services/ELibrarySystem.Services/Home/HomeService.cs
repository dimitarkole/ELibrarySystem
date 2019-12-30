namespace ELibrarySystem.Services.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ELibrarySystem.Data;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.Home;
    using ELibrarySystem.Web.ViewModels.HomeViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Logging;

    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext context;
        private readonly ISendMail sendMail;
        private readonly string vetyficationEmailType = "VeryfiUserCode";

        public HomeService(
            ApplicationDbContext context,
            ISendMail sendMail)
        {
            this.context = context;
            this.sendMail = sendMail;
        }

        public bool CheckVerifedEmail(string userEmail)
        {
            var check = this.context.Users
                .FirstOrDefault(u =>
                    u.Email == userEmail
                    && u.VerifiedOn == null);
            if (check == null)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        public void SendVerifyCodeToEmail(string userEmail)
        {
            var userId = this.context.Users.FirstOrDefault(u => u.Email == userEmail).Id;

            IdentityUserClaim<string> claim = this.context.UserClaims
                .FirstOrDefault(c =>
                    c.UserId == userId
                    && c.ClaimType == this.vetyficationEmailType);
            claim.UserId = userId;

            Random random = new Random();
            var length = 5;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string code = userId.Substring(0, 3);
            code += new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            claim.ClaimType = this.vetyficationEmailType;
            claim.ClaimValue = code;
            this.context.UserClaims.Add(claim);
            this.context.SaveChanges();

            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("code", code);
            this.sendMail.SendMailByTemplate(userEmail, "VerifyMailTemplate", info);
        }

        public bool VerifyEmail(VerifyEmailViewModel model)
        {
            var code = model.Code;
            var check = this.context.UserClaims
                .FirstOrDefault(c =>
                    c.ClaimValue == code
                    && c.ClaimType == this.vetyficationEmailType);
            if (check != null)
            {
                var userId = check.UserId;
                var user = this.context.Users.FirstOrDefault(u => u.Id == userId);
                user.VerifiedOn = DateTime.UtcNow;
                this.context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
