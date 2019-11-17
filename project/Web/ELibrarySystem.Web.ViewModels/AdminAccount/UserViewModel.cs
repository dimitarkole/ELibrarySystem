namespace ELibrarySystem.Web.ViewModels.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserViewModel
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string LibraryName { get; set; }

        public string Type { get; set; }
    }
}
