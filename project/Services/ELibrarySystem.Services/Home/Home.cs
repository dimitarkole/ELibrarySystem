namespace ELibrarySystem.Services.Home
{
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.Home;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Home : IHome
    {
        private ApplicationDbContext context;

        public Home(
            ApplicationDbContext context)
        {
            this.context = context;
        }
    }
}
