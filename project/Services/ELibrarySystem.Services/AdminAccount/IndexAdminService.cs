namespace ELibrarySystem.Services.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.AdminAccount;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.AdminAccount;

    public class IndexAdminService : IIndexAdminService
    {
        private ApplicationDbContext context;

        private IMessageService messageService;

        public IndexAdminService(
            ApplicationDbContext context,
            IMessageService messageService)
        {
            this.context = context;
            this.messageService = messageService;
        }

        public IndexAdminViewModel PreparedPage()
        {
            var indexAdminViewModel = new IndexAdminViewModel()
            {
                CountAdmins = this.CountUsersByType("admin"),
                CountAllUsers = this.CountAllUsers(),
                CountLibrarys = this.CountUsersByType("library"),
                CountUsers = this.CountUsersByType("user"),
            };
            return indexAdminViewModel;
        }

        private int CountAllUsers()
        {
            int count = this.context
                .Users
                .Where(u => u.DeletedOn == null)
                .Count();
            return count;
        }

        private int CountUsersByType(string type)
        {
            int count = this.context
                .Users
                .Where(u =>
                    u.Type == type
                    && u.DeletedOn == null)
                .Count();
            return count;
        }
    }
}
