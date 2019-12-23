namespace ELibrarySystem.Services.Contracts.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IProfileChakerService
    {
        public bool CheckCurrectAccount(string userId, string type);
    }
}
