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

        GiveBookViewModel GiveBookSearchUser(GiveBookViewModel model, string userId);

        GiveBookViewModel GiveBookChangeUserPage(GiveBookViewModel model, string userId, int newPage);

        GiveBookViewModel GiveBookSelectedBook(GiveBookViewModel model, string userId, string bookId);

        GiveBookViewModel GiveBookSelectedUser(GiveBookViewModel model, string userId);
    }
}
