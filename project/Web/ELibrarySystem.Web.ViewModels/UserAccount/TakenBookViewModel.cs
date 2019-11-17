﻿namespace ELibrarySystem.Web.ViewModels.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TakenBookViewModel
    {
        public TakenBookViewModel()
        {
            this.Id = null;
            this.LibraryId = null;
            this.Library = null;
            this.BookName = null;
            this.Author = null;
            this.Genre = null;
            this.GenreId = null;
            this.ReturnedOn = null;
        }

        public string Id { get; set; }

        public string LibraryId { get; set; }

        public LibraryViewModel Library { get; set; }

        public string BookName { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public string GenreId { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual DateTime? ReturnedOn { get; set; }
    }
}
