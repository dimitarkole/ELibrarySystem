namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public class GiveBookService : IGiveBookService
    {
        public ApplicationDbContext context;

        public IGetAllBooksServices getAllBooksServices;

        public IGenreService genreService;

        public IMessageService messageService;

        public GiveBookService(ApplicationDbContext context,
            IGenreService genreService,
            IMessageService messageService,
            IGetAllBooksServices getAllBooksServices)
        {
            this.context = context;
            this.genreService = genreService;
            this.messageService = messageService;
            this.getAllBooksServices = getAllBooksServices;
        }

        public GiveBookViewModel PreparedPage(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
