namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public GiveBookViewModel GiveBookChangeBookPage(GiveBookViewModel model, string userId, int newPage)
        {
            var allBooks = this.allBooksServices.ChangeActivePage(model.AllBooks, userId, newPage);
            var allUsers = model.AllUsers;
            var selectedBook = model.SelectedBook;
            var selectedUser = model.SelectedUser;
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
                SelectedBook = new BookViewModel(),
                SelectedUser = new UserViewModel(),
            };
            return returnModel;
        }

        public GiveBookViewModel GiveBookSearchBook(GiveBookViewModel model, string userId)
        {
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = model.AllUsers,
                SelectedBook = new BookViewModel(), //model.SelectedBook,
                SelectedUser = new UserViewModel(),
            };
            return returnModel;
        }

        public GiveBookViewModel GiveBookSearchUser(GiveBookViewModel model)
        {
            var allUsers = this.userService.GetUsers(model.AllUsers);
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = model.AllBooks,
                AllUsers = allUsers,
                SelectedBook = new BookViewModel(), //model.SelectedBook,
                SelectedUser = new UserViewModel(),
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
                SelectedBook = new BookViewModel(),
                SelectedUser = new UserViewModel(),
            };
            return model;
        }

        public GiveBookViewModel GiveBookChangeUserPage(GiveBookViewModel model, int newPage)
        {
            var allUsers = this.userService.ChangeActivePage(model.AllUsers, newPage);
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = model.AllBooks,
                AllUsers = allUsers,
                SelectedBook = new BookViewModel(),
                SelectedUser = new UserViewModel(),
            };
            return returnModel;
        }

        public GiveBookViewModel GiveBookSelectedBook(GiveBookViewModel model, string bookId)
        {
            var book = this.context.Books.FirstOrDefault(b => b.Id == bookId);
            var selectedBook = new BookViewModel()
            {
                Author = book.Author,
                BookId = book.Id,
                BookName = book.BookName,
                GenreName = book.Genre.Name,
                GenreId = book.GenreId,
            };
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = model.AllBooks,
                AllUsers = model.AllUsers,
                SelectedBook = selectedBook,
                SelectedUser = model.SelectedUser,
            };
            return returnModel;
        }

        public GiveBookViewModel GiveBookSelectedUser(GiveBookViewModel model, string userId)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id == userId);
            var selectedUser = new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = user.Id,
                Email = user.Email,
            };
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = model.AllBooks,
                AllUsers = model.AllUsers,
                SelectedBook = model.SelectedBook,
                SelectedUser = selectedUser,
            };
            return returnModel;
        }
    }
}
