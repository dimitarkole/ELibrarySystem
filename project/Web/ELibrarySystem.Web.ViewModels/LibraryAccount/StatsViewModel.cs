namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Web.ViewModels.SharedResources;

    public class StatsViewModel
    {
        public Book SearchBook { get; set; }

        public ChartViewModel ChartGettenBookSinceSixМonth { get; set; }

        public ChartViewModel ChartAddedBookSinceSixМonth { get; set; }
    }
}
