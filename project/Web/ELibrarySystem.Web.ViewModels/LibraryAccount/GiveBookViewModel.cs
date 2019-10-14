namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GiveBookViewModel
    {
        public BookViewModel SelectedBook;

        public UserViewModel SelectedUser;

        public AllBooksViewModel AllBooks;

        public AllUsersViewModel AllUsers;

        public GiveBookViewModel()
        {
            this.SelectedBook = new BookViewModel();
            this.SelectedUser = new UserViewModel();
            this.AllBooks = new AllBooksViewModel();
            this.AllUsers = new AllUsersViewModel();
        }
    }
}
