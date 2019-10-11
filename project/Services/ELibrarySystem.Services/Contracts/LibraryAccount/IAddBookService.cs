namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IAddBookService
    {
        AddBookViewModel PreparedPage();

        string AddBook(AddBookViewModel model, string userId);
    }
}
