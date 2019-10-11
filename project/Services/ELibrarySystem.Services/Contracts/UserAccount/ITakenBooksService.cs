namespace ELibrarySystem.Services.Contracts.UserAccount
{
    using ELibrarySystem.Web.ViewModels.UserAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITakenBooksService
    {
        TakenBooksViewModel PreparedPage(string userId);

        TakenBooksViewModel TakenBooks(TakenBooksViewModel model, string userId);

        TakenBooksViewModel ChangeActivePage(TakenBooksViewModel model, string userId, int newPage);
    }
}
