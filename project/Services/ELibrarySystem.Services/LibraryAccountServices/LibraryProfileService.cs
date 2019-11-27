namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using Microsoft.AspNetCore.Hosting.Internal;

    public class LibraryProfileService : ILibraryProfileService
    {
        private ApplicationDbContext context;
        private IMessageService messageService;

        public LibraryProfileService(
            ApplicationDbContext context,
            IMessageService messageService)
        {
            this.context = context;
            this.messageService = messageService;
        }

        public ProfilLibraryViewModel PreparedPage(string userId)
        {
            var user = this.context.Users
                .FirstOrDefault(u =>
               u.DeletedOn == null
               && u.Id == userId);
            ProfilLibraryViewModel profil = new ProfilLibraryViewModel();
            if (user != null)
            {
                profil.Avatar = user.Avatar;
                profil.LibararyName = user.LibararyName;
                profil.LibraryLocation = user.LibraryLocation;
            }

            return profil;
        }

        public List<object> SaveChanges(ProfilLibraryViewModel model, string userId)
        {
            var user = this.context.Users
                .FirstOrDefault(u =>
               u.DeletedOn == null
               && u.Id == userId);
            ProfilLibraryViewModel profil = new ProfilLibraryViewModel();
            var result = new List<object>();
            string resultTitle = "Неуспешно редактиран профил";
            if (user != null)
            {
                string uploadFile = Path.Combine()


                user.Avatar = model.Avatar;
                user.LibararyName = model.LibararyName;
                user.LibraryLocation = model.LibraryLocation;
                this.context.SaveChanges();
                resultTitle = "Успешно редактиран профил";
                this.messageService.AddMessageAtDB(userId, resultTitle);
            }

            result.Add(this.PreparedPage(userId));
            result.Add(resultTitle);
            return result;
        }
    }
}
