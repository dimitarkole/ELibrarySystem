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
            else
            {
                return true;
            }
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

            userEmail = "dim_kolev2002@abv.bg";

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
    }
}
