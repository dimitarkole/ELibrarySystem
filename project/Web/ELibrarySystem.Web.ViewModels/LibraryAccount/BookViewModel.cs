namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BookViewModel
    {
        public BookViewModel()
        {
            this.BookName = null;
            this.Author = null;
            this.GenreName = null;
            this.GenreId = null;
            this.BookId = null;
        }

        public string BookName { get; set; }

        public string Author { get; set; }

        public string GenreName { get; set; }

        public string GenreId { get; set; }

        public string BookId { get; set; }

    }
}
