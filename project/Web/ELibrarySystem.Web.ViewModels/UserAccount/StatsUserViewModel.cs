namespace ELibrarySystem.Web.ViewModels.UserAccount
{
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StatsUserViewModel
    {

        public StatsUserViewModel()
        {
            this.SearchBook = new Book();
        }

        public Book SearchBook { get; set; }

        public List<GenreListViewModel> Genres { get; set; }

        public ChartGettenBookSinceSixМonth ChartGettenBookSinceSixМonth { get; set; }

    }
}
