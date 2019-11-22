namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IStatsLibraryService
    {
        StatsLibaryViewModel PreparedPage(string userId);

        StatsLibaryViewModel SearchStats(StatsLibaryViewModel model, string userId);

    }
}
