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
    using ELibrarySystem.Web.ViewModels.UserAccount;

    public class AdminProfileService : IAdminProfileService
    {
        private ApplicationDbContext context;

        private IMessageService messageService;

        public AdminProfileService(
            ApplicationDbContext context,
            IMessageService messageService)
        {
            this.context = context;
            this.messageService = messageService;
        }

        public ProfilAdminViewModel PreparedPage(string userId)
        {
            var user = this.context
             .Users
             .FirstOrDefault(u => u.Id == userId);
            var model = new ProfilAdminViewModel()
            {
                AvatarLocation = user.Avatar,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            return model;
        }

        public ProfilAdminViewModel SaveChanges(ProfilAdminViewModel model, string userId)
        {
            var user = this.context.Users
                .FirstOrDefault(u => u.Id == userId);
            user.Avatar = model.AvatarLocation;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            this.context.SaveChanges();

            var message = $"Успешно променен профил";
            this.messageService.AddMessageAtDB(userId, message);

            var returnModel = this.PreparedPage(userId);
            return returnModel;
        }
    }
}
