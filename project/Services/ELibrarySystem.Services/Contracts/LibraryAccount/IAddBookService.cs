using ELibrarySystem.Web.ViewModels.LibraryAccount;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    public interface IAddBookService
    {
        AddBookViewModel PreparedPage();

        string AddBook(AddBookViewModel model, string userId);
    }
}
