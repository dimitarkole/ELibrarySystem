namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.LibraryAccount;   

    public interface IIndexService
    {
        IndexViewModel PreparedPage(string userId);
    }
}
