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
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);
            var allUsers = model.AllUsers;
            var selectedBook = model.SelectedBook;
            var selectedUser = model.SelectedUser;
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
                SelectedBook = selectedBook,
                SelectedUser = selectedUser,
            };
            return returnModel;
        }

        public GiveBookViewModel PreparedPage(string userId)
        {
            var allBooks = this.allBooksServices.PreparedPage(userId);
            var allUsers = this.userService.PreparedPage();
            var model = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
            };
            return model;
        }
    }
}
