namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserViewModel
    {
        public UserViewModel()
        {
            this.UserName = null;
            this.FirstName = null;
            this.LastName = null;
            this.Email = null;
            this.UserId = null;
        }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email{ get; set; }

        public string UserId { get; set; }
    }
}
