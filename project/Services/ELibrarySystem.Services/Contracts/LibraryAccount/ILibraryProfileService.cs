namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ILibraryProfileService
    {
        ProfilLibraryViewModel PreparedPage(string userId);

        List<object> SaveChanges(ProfilLibraryViewModel model,string userId);

    }
}
