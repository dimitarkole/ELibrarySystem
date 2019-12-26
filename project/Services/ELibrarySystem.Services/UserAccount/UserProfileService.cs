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
        private ApplicationDbContext context;

        private IMessageService messageService;

        public UserProfileService(
            ApplicationDbContext context,
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
                AvatarLocation = user.Avatar,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
            return model;
        }

        public ProfilUserViewModel SaveChanges(ProfilUserViewModel model, string userId)
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

        public List<object> ChangeType(string userId)
        {
            var result = new List<object>();
            result.Add(this.PreparedPage(userId));
            result.Add(this.ChangeTypeSendMessages(userId));
            return result;
        }

        private string ChangeTypeSendMessages(string userId)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id == userId);
            var admins = this.context.Users.Where(u => u.Type == "admin").ToList();
            var message = "Искам да стана библиотека - email =" + user.Email;
            foreach (var admin in admins)
            {
                this.messageService.AddMessageAtDB(admin.Id, message);
            }

            return "Успешно изпратено заявление за промена на типа на акаунта!";
        }
    }
}
