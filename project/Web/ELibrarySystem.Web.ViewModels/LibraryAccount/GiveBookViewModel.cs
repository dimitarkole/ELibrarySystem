namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GiveBookViewModel
    {
        public BookViewModel SelectedBook { get; set; }

        public UserViewModel SelectedUser { get; set; }

        public AllBooksViewModel AllBooks { get; set; }

        public AllUsersViewModel AllUsers { get; set; }

      
    }
}
