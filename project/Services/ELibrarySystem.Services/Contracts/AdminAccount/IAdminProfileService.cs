namespace ELibrarySystem.Services.Contracts.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ELibrarySystem.Web.ViewModels.AdminAccount;
    using ELibrarySystem.Web.ViewModels.UserAccount;

    public interface IAdminProfileService
    {
        ProfilAdminViewModel PreparedPage(string userId);

        ProfilAdminViewModel SaveChanges(ProfilAdminViewModel model, string userId);
    }
}
