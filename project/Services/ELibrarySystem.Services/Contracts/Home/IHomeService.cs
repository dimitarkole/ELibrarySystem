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
        public bool CheckVerifedEmail(string userEmail);

        public void SendVerifyCodeToEmail(string userEmail);

        public bool VerifyEmail(VerifyEmailViewModel model);
    }
}
