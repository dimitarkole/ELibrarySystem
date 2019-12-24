namespace ELibrarySystem.Web.ViewModels.AdminAccount
{
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.SharedResources;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StatsAdminViewModel
    {

        public StatsAdminViewModel()
        {
            this.SearchUser = new ApplicationUser();
        }

        public ApplicationUser SearchUser { get; set; }

        public ChartAddedUsers ChartAddedUsers { get; set; }

        public ChartViewModel ChartAddedBookSinceSixМonth { get; set; }

    }
}
