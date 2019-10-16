namespace ELibrarySystem.Web.ViewModels.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TakenBookViewModel
    {
        public string Id { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public string GenreId { get; set; }


        public virtual DateTime CreatedOn { get; set; }

        public virtual DateTime? ReturnedOn { get; set; }
    }
}
