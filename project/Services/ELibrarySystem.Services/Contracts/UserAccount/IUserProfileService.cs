﻿namespace ELibrarySystem.Services.Contracts.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.UserAccount;

    public interface IUserProfileService
    {
        ProfilUserViewModel PreparedPage(string userId);

        ProfilUserViewModel SaveChanges(ProfilUserViewModel model, string userId);

        List<object> ChangeType (string userId);

        List<object> ResetPassword(ProfilUserViewModel model, string userId);

    }
}
