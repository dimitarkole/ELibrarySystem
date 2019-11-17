namespace ELibrarySystem.Services.Contracts.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.AdminAccount;

    public interface IUsersService
    {
        UsersViewModel PreparedPage();

        List<object> MakeUserLibrary(UsersViewModel model, string userId);

        List<object> MakeLibraryUser(UsersViewModel model, string userId);

        List<object> MakeUserAdmin(UsersViewModel model, string userId);

        List<object> DeleteUser(UsersViewModel model, string userId);

        UsersViewModel ChangeActivePage(
         UsersViewModel model,
         int newPage);

    }
}
