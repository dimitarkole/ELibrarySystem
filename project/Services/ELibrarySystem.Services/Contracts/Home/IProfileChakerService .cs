namespace ELibrarySystem.Services.Contracts.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IProfileChakerService
    {
        public string CheckCurrectAccount(string userId, string type);
    }
}
