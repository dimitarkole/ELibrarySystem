namespace ELibrarySystem.Services.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Services.Contracts.UserAccount;
    using ELibrarySystem.Web.ViewModels.UserAccount;

    public class UserProfileService : IUserProfileService
    {
        public ApplicationDbContext context;
        public IMessageService messageService;

        public UserProfileService(ApplicationDbContext context,
            IMessageService messageService)
        {
            this.context = context;
            this.messageService = messageService;
        }

        public ProfilUserViewModel PreparedPage(string userId)
        {
            var user = this.context
             .Users
             .FirstOrDefault(u => u.Id == userId);
            var model = new ProfilUserViewModel()
            {
                Avatar = user.Avatar,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            return model;
        }

        public ProfilUserViewModel SaveChanges(ProfilUserViewModel model, string userId)
        {
            var user = this.context
           .Users
           .FirstOrDefault(u => u.Id == userId);
            user.Avatar = model.Avatar;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            this.context.SaveChanges();

            var returnModel = this.PreparedPage(userId);
            return returnModel;
        }
    }
}
