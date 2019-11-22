namespace ELibrarySystem.Services.Contracts.UserAccount
{
    using ELibrarySystem.Web.ViewModels.UserAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IStatsUserService
    {
        StatsUserViewModel PreparedPage(string userId);

        StatsUserViewModel SearchStats(StatsUserViewModel model, string userId);
    }
}
