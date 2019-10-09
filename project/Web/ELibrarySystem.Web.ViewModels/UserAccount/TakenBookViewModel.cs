namespace ELibrarySystem.Web.ViewModels.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TakenBookViewModel
    {
        public string BookName { get; set; }

        public string Author { get; set; }

        public string GenreName { get; set; }

        public string GenreId { get; set; }

        public string GetBookId { get; set; }

        public DateTime? Returned { get; set; }
    }
}
