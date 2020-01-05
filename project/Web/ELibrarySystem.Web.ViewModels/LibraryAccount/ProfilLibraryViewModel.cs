namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.ComponentModel;
    using System.Web;
    using Microsoft.AspNetCore.Http;
    using ELibrarySystem.Web.ViewModels.SharedResources;

    public class ProfilLibraryViewModel
    {
        public ProfilLibraryViewModel()
        {
            this.ResetPasswordViewModel = new ResetPasswordViewModel();
        }

        public string AvatarLocation { get; set; }

        public string LibararyName { get; set; }

        public string LibraryLocation { get; set; }

        public IFormFile Photo { get; set; }

        public ResetPasswordViewModel ResetPasswordViewModel { get; set; }
    }
}
