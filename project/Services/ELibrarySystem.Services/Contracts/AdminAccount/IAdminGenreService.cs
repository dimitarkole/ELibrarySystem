namespace ELibrarySystem.Services.Contracts.AdminAccount
{
    using ELibrarySystem.Web.ViewModels.AdminAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IAdminGenreService
    {

        string AddGenre(AddGenreViewModel model);
    }
}
