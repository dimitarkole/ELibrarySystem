namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IMessageService
    {
        string AddMessageAtDB(string userId, string textOfMessage);
    }
}
