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

        public bool CheckVerifedEmail(string userId)
        {
            var check = this.context.Users
                .FirstOrDefault(u =>
                    u.Id == userId).VerifiedOn;
            if (check == null)
            {
                return false;
            }

            return true;
        }

        public string ForgotenPasswordSendCode(ForgotenPasswordViewModel model)
        {
            var checkEmailAtDB = this.context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (checkEmailAtDB == null)
            {
                return "Няма регистриран потребител с такъв email";
            }

            var userId = checkEmailAtDB.Email;
            var claimType = "ForgotenPasswordSendCode";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string code = userId.Substring(0, Math.Min(3, userId.Length));
            var length = 8 - code.Length;
            Random random = new Random();
            code += new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var claim = new IdentityUserClaim<string>();
            var checkClaimCode = this.context.UserClaims
             .FirstOrDefault(c => c.UserId == userId
              && c.ClaimType == claimType);
            if (checkClaimCode != null)
            {
                claim = checkClaimCode;
            }

            claim.ClaimType = claimType;
            claim.ClaimValue = code;
            claim.UserId = userId;
            this.context.SaveChanges();

            var info = new Dictionary<string, string>();
            info.Add("code", code);
            var userEmail = model.Email;

            this.sendMail.SendMailByTemplate(userEmail, claimType, info);
            return " ";
        }

        public IndexViewModel GetDataForIndexPage()
        {
            var model = new IndexViewModel()
            {
                CountAddedBook = this.CountAddedBook(),
                CountLibraries = this.CountLibraries(),
                CountReaders = this.CountReaders(),
                CountReadBook = this.CountReadBook(),

            };
            return model;
        }

        public void SendVerifyCodeToEmail(string userId)
        {
            var checkVerificatedCode = this.context.VerificatedCodes
                .FirstOrDefault(vc => vc.UserId == userId);
            var verificatedCode = new VerificatedCode();
            if (checkVerificatedCode != null)
            {
                verificatedCode = checkVerificatedCode;
            }

            verificatedCode.UserId = userId;

            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string code = userId.Substring(0, Math.Min(3, userId.Length));
            var length = 8 - code.Length;

            code += new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            verificatedCode.Code = code;
            if (checkVerificatedCode == null)
            {
                this.context.VerificatedCodes.Add(verificatedCode);
            }

            this.context.SaveChanges();

            Dictionary<string, string> info = new Dictionary<string, string>();
            info.Add("code", code);
            var userEmail = this.context.Users.FirstOrDefault(u => u.Id == userId).Email;
            this.sendMail.SendMailByTemplate(userEmail, "VerifyMailTemplate", info);
        }

        public Dictionary<string, string> VerifyEmail(VerifyEmailViewModel model)
        {
            var code = model.Code;
            var check = this.context.VerificatedCodes
                .FirstOrDefault(vf => vf.Code == code);
            var result = new Dictionary<string, string>();
            result.Add("verificating", "No");
            if (check != null)
            {
                var userId = check.UserId;
                var user = this.context.Users.FirstOrDefault(u => u.Id == userId);
                user.VerifiedOn = DateTime.UtcNow;
                this.context.SaveChanges();
                result["verificating"] = "Yes";
                result.Add("userId", user.Id);
                result.Add("type", user.Type);

            }

            return result;
        }

        private int CountAddedBook()
        {
            var count = this.context.Books
                .Where(b => b.DeletedOn == null)
                .Count();
            return count;
        }

        private int CountLibraries()
        {
            var count = this.context.Users
                .Where(u => u.DeletedOn == null && u.Type == "library")
                .Count();
            return count;
        }

        private int CountReaders()
        {
            var count = this.context.Users
                .Where(u => u.DeletedOn == null && u.Type == "user")
                .Count();
            return count;
        }

        private int CountReadBook()
        {
            var count = this.context.GetBooks
                .Where(gb => gb.DeletedOn == null)
                .Count();
            return count;
        }

    }
}
