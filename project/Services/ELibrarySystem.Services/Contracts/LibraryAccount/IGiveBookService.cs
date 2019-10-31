namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public interface IGiveBookService
    {
        GiveBookViewModel PreparedPage(string userId);

        GiveBookViewModel GiveBookSearchBook(
            GiveBookViewModel model, 
            string userId,
            string selectedBookId,
            string selectedUserId);

        GiveBookViewModel GiveBookChangeBookPage(
            GiveBookViewModel model,
            string userId,
            int newPage,
            string selectedBookId,
            string selectedUserId);

        GiveBookViewModel GiveBookSearchUser(
            GiveBookViewModel model,
            string userId,
            string selectedBookId,
            string selectedUserId);

        GiveBookViewModel GiveBookChangeUserPage(
            GiveBookViewModel model,
            string userId,
            int newPage,
            string selectedBookId,
            string selectedUserId);

        GiveBookViewModel GiveBookSelectedBook(
            GiveBookViewModel model,
            string userId,
            string bookId,
            string selectedUserId);

        GiveBookViewModel GiveBookSelectedUser(
            GiveBookViewModel model,
            string userId,
            string selectUserId,
            string selectedBookId);

        GiveBookViewModel GivingBook(
          GiveBookViewModel model,
          string userId,
          string selectedBookId,
          string selectedUserId);

        GiveBookViewModel EditingGivinBook(
          GiveBookViewModel model,
          string userId,
          string givenBookId,
          string selectedBookId,
          string selectedUserId);
    }
}
