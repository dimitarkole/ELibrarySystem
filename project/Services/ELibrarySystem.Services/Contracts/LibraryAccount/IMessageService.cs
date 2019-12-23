namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.SharedResources;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IMessageService
    {
        string AddMessageAtDB(string userId, string textOfMessage);

        MessagesNavBarViewModel GetMessagesNavBar(string userId);

        MessagesViewModel GetMessagesPreparedPage(string userId);


        MessagesViewModel GetMessagesChangePage(MessagesViewModel model, string userId, int pageIndex);

    }
}
