namespace ELibrarySystem.Web.ViewModels.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class LibraryViewModel
    {

        public LibraryViewModel()
        {

        }

        public LibraryViewModel(string libraryName, string libraryMail)
        {
            this.LibraryName = libraryName;
            this.LibraryEmail = libraryMail;

        }

        public string LibraryName { get; set; }

        public string LibraryEmail { get; set; }
    }
}
