namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IGivenBooksService
    {
        GivenBooksViewModel PreparedPage(string userId);

        GivenBooksViewModel GetGevenBooks(GivenBooksViewModel model, string userId);

        List<object> ReturnBook(string userId, string givenBookId);

        GivenBooksViewModel ChangeActivePage(GivenBooksViewModel model, string userId, int newPage);


    }
}
