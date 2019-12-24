namespace ELibrarySystem.Web.ViewModels.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.AspNetCore.Http;

    public class ProfilAdminViewModel
    {
        public string AvatarLocation { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IFormFile Photo { get; set; }
    }
}
