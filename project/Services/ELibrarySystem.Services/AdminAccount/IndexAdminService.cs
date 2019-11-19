namespace ELibrarySystem.Services.AdminAccount
{
    using System;
    using System.Collections.Generic;
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

        public IndexAdminViewModel PreparedPage(string userId)
        {
            var indexAdminViewModel = new IndexAdminViewModel();

            return indexAdminViewModel;
        }
    }
}
