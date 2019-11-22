namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Web.ViewModels.SharedResources;

    public class StatsViewModel
    {
        public StatsViewModel()
        {
            this.SearchBook = new Book();
        }

        public Book SearchBook { get; set; }

        public List<GenreListViewModel> Genres { get; set; }

        public ChartGettenBookSinceSixМonth ChartGettenBookSinceSixМonth { get; set; }

        public ChartViewModel ChartAddedBookSinceSixМonth { get; set; }
    }
}
