namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ELibrarySystem.Data;
    using ELibrarySystem.Data.Models;
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

        public GiveBookViewModel GiveBookSearchBook(
            GiveBookViewModel model,
            string userId,
            string selectedBookId,
            string selectedUserId)
        {
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);
            var allUsers = this.userService.GetUsers(model.AllUsers);
            var selectedUser = this.SelectingUser(selectedUserId);
            var selectedBook = this.SelectingBook(selectedBookId);
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
                SelectedBook = selectedBook,
                SelectedUser = selectedUser,
            };
            return returnModel;
        }

        public GiveBookViewModel GiveBookChangeBookPage(
            GiveBookViewModel model,
            string userId,
            int newPage,
            string selectedBookId,
            string selectedUserId)
        {
            var allBooks = this.allBooksServices.ChangeActivePage(model.AllBooks, userId, newPage);
            var allUsers = this.userService.GetUsers(model.AllUsers);
            var selectedUser = this.SelectingUser(selectedUserId);
            var selectedBook = this.SelectingBook(selectedBookId);
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
                SelectedBook = selectedBook,
                SelectedUser = selectedUser,
            };
            return returnModel;
        }

        public GiveBookViewModel GiveBookSearchUser(
            GiveBookViewModel model,
            string userId,
            string selectedBookId,
            string selectedUserId)
        {
            var allUsers = this.userService.GetUsers(model.AllUsers);
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);
            var selectedUser = this.SelectingUser(selectedUserId);
            var selectedBook = this.SelectingBook(selectedBookId);
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
                SelectedBook = selectedBook,
                SelectedUser = selectedUser,
            };
            return returnModel;
        }

        public GiveBookViewModel GiveBookChangeUserPage(
            GiveBookViewModel model,
            string userId,
            int newPage,
            string selectedBookId,
            string selectedUserId)
        {
            var allUsers = this.userService.ChangeActivePage(model.AllUsers, newPage);
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);
            var selectedUser = this.SelectingUser(selectedUserId);
            var selectedBook = this.SelectingBook(selectedBookId);
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
                SelectedBook = selectedBook,
                SelectedUser = selectedUser,
            };
            return returnModel;
        }

        public GiveBookViewModel GiveBookSelectedBook(
            GiveBookViewModel model,
            string userId,
            string bookId,
            string selectedUserId)
        {
            var book = this.SelectingBook(bookId);
            var selectedUser = this.SelectingUser(selectedUserId);

            var allUsers = this.userService.GetUsers(model.AllUsers);
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);

            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
                SelectedBook = book,
                SelectedUser = selectedUser,
            };
            return returnModel;
        }

        public GiveBookViewModel GiveBookSelectedUser(
           GiveBookViewModel model,
           string userId,
           string selectUserId,
           string selectedBookId)
        {

            var user = this.SelectingUser(selectUserId);
            var selectedBook = this.SelectingBook(selectedBookId);

            var allUsers = this.userService.GetUsers(model.AllUsers);
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);

            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
                SelectedBook = selectedBook,
                SelectedUser = user,
            };
            return returnModel;
        }

        public GiveBookViewModel GivingBook(
            GiveBookViewModel model,
            string userId,
            string selectedBookId,
            string selectedUserId)
        {
            var allUsers = this.userService.GetUsers(model.AllUsers);
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);
            var selectedUser = this.SelectingUser(selectedUserId);
            var selectedBook = this.SelectingBook(selectedBookId);

            var book = this.context.Books.FirstOrDefault(b => b.Id == selectedBookId);
            var user = this.context.Users.FirstOrDefault(u => u.Id == selectedUserId);
            GetBook getBook = new GetBook()
            {
                Book = book,
                BookId = selectedUserId,
                User = user,
                UserId = selectedUserId,
            };
            this.context.GetBooks.Add(getBook);
            this.context.SaveChanges(); 
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
                SelectedBook = selectedBook,
                SelectedUser = selectedUser,
            };
            return returnModel;
        }

        public GiveBookViewModel EditintGivinBook(
        GiveBookViewModel model,
        string userId,
        string givenBookId,
        string selectedBookId,
        string selectedUserId)
        {
            var allUsers = this.userService.GetUsers(model.AllUsers);
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);
            var selectedUser = this.SelectingUser(selectedUserId);
            var selectedBook = this.SelectingBook(selectedBookId);

            var getBook = this.context.GetBooks
                .FirstOrDefault(gb => gb.Id == givenBookId);
            var book = this.context.Books.FirstOrDefault(b => b.Id == selectedBookId);
            var user = this.context.Users.FirstOrDefault(u => u.Id == selectedUserId);

            if (getBook != null)
            {
                getBook.Book = book;
                getBook.BookId = selectedUserId;
                getBook.User = user;
                getBook.UserId = selectedUserId;
                this.context.SaveChanges();
            }

            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
                SelectedBook = selectedBook,
                SelectedUser = selectedUser,
            };
            return returnModel;
        }

        private BookViewModel SelectingBook(string bookId)
        {
            var book = this.context.Books.FirstOrDefault(b => b.Id == bookId);
            var selectedBook = new BookViewModel();
            if (book != null)
            {

                string genreName = this.context.Genres
                    .FirstOrDefault(g => g.Id == book.GenreId)
                    .Name;
                selectedBook.Author = book.Author;
                selectedBook.BookId = book.Id;
                selectedBook.BookName = book.BookName;
                selectedBook.GenreName = genreName;
                selectedBook.GenreId = book.GenreId;
            }

            return selectedBook;
        }

        private UserViewModel SelectingUser(string userId)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id == userId);

            var selectedUser = new UserViewModel();
            if (user != null)
            {
                selectedUser.FirstName = user.FirstName;
                selectedUser.LastName = user.LastName;
                selectedUser.UserId = user.Id;
                selectedUser.Email = user.Email;
            }

            return selectedUser;
        }

    
    }
}
