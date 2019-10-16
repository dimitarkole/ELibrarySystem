namespace ELibrarySystem.Services.Contracts.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.UserAccount;

    public interface ITakenBooksService
    {
        TakenBooksViewModel PreparedPage(string userId);

        TakenBooksViewModel TakenBooks(TakenBooksViewModel model, string userId);

        TakenBooksViewModel ChangeActivePage(TakenBooksViewModel model, string userId, int newPage);
    }
}
