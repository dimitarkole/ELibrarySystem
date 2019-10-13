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

        public IAllBooksServices allBooksServices;

        public IUserService userService;

        public IMessageService messageService;

        public GiveBookService(ApplicationDbContext context,
            IUserService userService,
            IMessageService messageService,
            IAllBooksServices allBooksServices)
        {
            this.context = context;
            this.userService = userService;
            this.messageService = messageService;
            this.allBooksServices = allBooksServices;
        }

        public GiveBookViewModel GiveBookSearchBook(GiveBookViewModel model, string userId)
        {
            var returnModel = new GiveBookViewModel();
            returnModel.AllBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);
            returnModel.AllUsers = model.AllUsers;
            return returnModel;
        }

        public GiveBookViewModel PreparedPage(string userId)
        {
            var model = new GiveBookViewModel();
            model.AllBooks = this.allBooksServices.PreparedPage(userId);
            model.AllUsers = this.userService.PreparedPage();
            return model;
        }
    }
}
