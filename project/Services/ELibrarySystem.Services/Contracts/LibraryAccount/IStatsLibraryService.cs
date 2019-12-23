namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.LibraryAccount; 

    public interface IStatsLibraryService
    {
        StatsLibaryViewModel PreparedPage(string userId);

        StatsLibaryViewModel SearchStats(StatsLibaryViewModel model, string userId);
    }
}
