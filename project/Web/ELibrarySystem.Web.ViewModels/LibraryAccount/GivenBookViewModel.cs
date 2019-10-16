namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;

    public class GivenBookViewModel
    {
        public string Id { get; set; }

        public string BookName { get; set; }

        public string BookAuthor { get; set; }

        public string BookGenre { get; set; }

        public string BookGenreId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }


        public virtual DateTime CreatedOn { get; set; }

        public virtual DateTime? ReturnedOn { get; set; }

    }
}
