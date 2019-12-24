namespace ELibrarySystem.Services.Contracts.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.AdminAccount;

    public interface IStatsAdminService
    {
        StatsAdminViewModel PreparedPage(string userId);

        StatsAdminViewModel SearchStats(StatsAdminViewModel model, string userId);
    }
}
