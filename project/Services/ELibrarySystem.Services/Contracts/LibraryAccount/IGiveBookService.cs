namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public interface IGiveBookService
    {
        GiveBookViewModel PreparedPage(string userId);
    }
}
