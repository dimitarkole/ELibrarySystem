namespace ELibrarySystem.Web.ViewModels.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.AspNetCore.Http;

    public class ProfilUserViewModel
    {
        public string AvatarLocation { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IFormFile Photo { get; set; }
    }
}
