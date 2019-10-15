namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public interface IGiveBookService
    {
        GiveBookViewModel PreparedPage(string userId);

        GiveBookViewModel GiveBookSearchBook(GiveBookViewModel model, string userId);

        GiveBookViewModel GiveBookChangeBookPage(GiveBookViewModel model, string userId, int newPage);

        GiveBookViewModel GiveBookSearchUser(GiveBookViewModel model);

        GiveBookViewModel GiveBookChangeUserPage(GiveBookViewModel model, int newPage);


    }
}
