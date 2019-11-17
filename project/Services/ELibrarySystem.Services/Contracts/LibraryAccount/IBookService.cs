namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IBookService
    {
        AddBookViewModel PreparedAddBookPage();

        string AddBook(AddBookViewModel model, string userId);

        AddBookViewModel GetBookDataById(string bookId);

        List<object> EditBook(AddBookViewModel model, string userId);

      
    }
}
