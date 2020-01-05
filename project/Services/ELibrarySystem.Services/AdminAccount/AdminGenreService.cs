namespace ELibrarySystem.Services.AdminAccount
{
    using ELibrarySystem.Data;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.AdminAccount;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.AdminAccount;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AdminGenreService : IAdminGenreService
    {
        private ApplicationDbContext context;

        private IMessageService messageService;

        public AdminGenreService(
            ApplicationDbContext context,
            IMessageService messageService)
        {
            this.context = context;
            this.messageService = messageService;
        }

        public string AddGenre(AddGenreViewModel model)
        {
            var checherGenre = this.context.Genres.FirstOrDefault(g => g.Name == model.GenreName);
            if (checherGenre == null)
            {
                Genre genre = new Genre()
                {
                    Name = model.GenreName,
                };
                this.context.Genres.Add(genre);
                return "Успершно добавяне на жанр!";
            }

            return "Този жанр вече съществува в базата данни!";
        }
    }
}
