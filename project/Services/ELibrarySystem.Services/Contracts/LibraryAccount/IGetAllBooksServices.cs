using ELibrarySystem.Web.ViewModels.LibraryAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
 
    public interface IGetAllBooksServices
    {
        AllBooksViewModel PreparedPage(string userId);

        AllBooksViewModel GetBooks(AllBooksViewModel model, string userId);

        AllBooksViewModel DeleteBook(string userId, AllBooksViewModel model, string bookId);

        AllBooksViewModel ChangeActivePage(AllBooksViewModel model, string userId, int newPage);
    }
}
