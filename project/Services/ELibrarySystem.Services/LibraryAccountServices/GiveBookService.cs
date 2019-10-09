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

        public IUserService userService;

        public IMessageService messageService;

        public GiveBookService(ApplicationDbContext context,
            IUserService userService,
            IMessageService messageService,
            IGetAllBooksServices getAllBooksServices)
        {
            this.context = context;
            this.userService = userService;
            this.messageService = messageService;
            this.getAllBooksServices = getAllBooksServices;
        }

        public GiveBookViewModel PreparedPage(string userId)
        {
            var model = new GiveBookViewModel();
            model.AllBooks = this.getAllBooksServices.PreparedPage(userId);
            model.AllUsers = this.userService.PreparedPage();
            return model;
        }
    }
}
