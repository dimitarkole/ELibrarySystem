namespace ELibrarySystem.Services.Home
{
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.Home;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ProfileChakerService : IProfileChakerService
    {
        private ApplicationDbContext context;

        public ProfileChakerService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool CheckCurrectAccount(string userId, string type)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id == userId);
            if (type == user.Type)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
