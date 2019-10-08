namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IBookService
    {
        AddBookViewModel PreparedPage();

        string AddBook(AddBookViewModel model, string userId);

        AddBookViewModel GetBookData(string bookId);

        string EditBook(AddBookViewModel model, string userId);

    }
}
