namespace ELibrarySystem.Services.Contracts.UserAccount
{
    using ELibrarySystem.Web.ViewModels.UserAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public interface IIndexUserService
    {
        IndexUserViewModel PreparedPage(string userId);
    }
}
