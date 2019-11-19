namespace ELibrarySystem.Services.Contracts.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.AdminAccount;

    public interface IIndexAdminService
    {
        IndexAdminViewModel PreparedPage(string userId);
    }
}
