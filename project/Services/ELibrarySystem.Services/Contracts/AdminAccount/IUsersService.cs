﻿namespace ELibrarySystem.Services.Contracts.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.AdminAccount;

    public interface IUsersService
    {
        UsersViewModel PreparedPage(string userId);
    }
}
