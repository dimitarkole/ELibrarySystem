namespace ELibrarySystem.Web.ViewModels.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ELibrarySystem.Web.ViewModels.SharedResources;
    using Microsoft.AspNetCore.Http;

    public class ProfilUserViewModel
    {
        public ProfilUserViewModel()
        {
            this.ResetPasswordViewModel = new ResetPasswordViewModel();
        }

        public string AvatarLocation { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IFormFile Photo { get; set; }

        public ResetPasswordViewModel ResetPasswordViewModel { get; set; }
    }
}
