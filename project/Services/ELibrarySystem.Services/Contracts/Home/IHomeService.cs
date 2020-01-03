namespace ELibrarySystem.Services.Contracts.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using ELibrarySystem.Data;
    using ELibrarySystem.Web.ViewModels.HomeViewModels;

    public interface IHomeService
    {
        public bool CheckVerifedEmail(string userId);

        public void SendVerifyCodeToEmail(string userId);

        public Dictionary<string, string> VerifyEmail(VerifyEmailViewModel model);

        public string ForgotenPasswordSendCode(ForgotenPasswordViewModel model);
    }
}
