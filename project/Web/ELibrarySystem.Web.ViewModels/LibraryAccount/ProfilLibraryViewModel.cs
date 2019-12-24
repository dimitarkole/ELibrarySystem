namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.ComponentModel;
    using System.Web;
    using Microsoft.AspNetCore.Http;

    public class ProfilLibraryViewModel
    {
        public string AvatarLocation { get; set; }

        public string LibararyName { get; set; }

        public string LibraryLocation { get; set; }

        public IFormFile Photo { get; set; }
    }
}
