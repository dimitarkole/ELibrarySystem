using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    public interface IMessageService
    {
        string AddMessageAtDB(string userId, string textOfMessage);
    }
}
