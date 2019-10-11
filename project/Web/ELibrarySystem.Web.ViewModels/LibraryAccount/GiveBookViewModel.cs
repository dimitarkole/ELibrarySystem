namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GiveBookViewModel
    {
        public AllBooksViewModel AllBooks;

        public AllUsersViewModel AllUsers;

        public GiveBookViewModel()
        {
            this.AllBooks = new AllBooksViewModel();
            this.AllUsers = new AllUsersViewModel();
        }
    }
}
