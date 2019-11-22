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
        private ApplicationDbContext context;

        private IAllBooksServices allBooksServices;

        private IUserService userService;

        private IMessageService messageService;

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

        public List<object> GivingBook(
            GiveBookViewModel model,
            string userId,
            string selectedBookId,
            string selectedUserId)
        {
            var chackInputData = this.ChackInputData(selectedUserId, selectedBookId);
            List<object> result = new List<object>();
            var allUsers = this.userService.GetUsers(model.AllUsers);
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
            };
            if (string.IsNullOrEmpty(chackInputData))
            {
                var selectedUser = this.SelectingUser(selectedUserId);
                var selectedBook = this.SelectingBook(selectedBookId);

                var book = this.context.Books.FirstOrDefault(b => b.Id == selectedBookId);
                var user = this.context.Users.FirstOrDefault(u => u.Id == selectedUserId);
                GetBook getBook = new GetBook()
                {
                    Book = book,
                    BookId = selectedBookId,
                    User = user,
                    UserId = selectedUserId,
                };
                this.context.GetBooks.Add(getBook);
                this.context.SaveChanges();

                var library = this.context.Users.FirstOrDefault(u => u.Id == userId);
                var message = $"Успешно дадена книга от {library.LibararyName} - {library.Email}!";
                this.messageService.AddMessageAtDB(selectedUserId, message);

                message = $"Успешно дадена книгана на {user.FirstName} {user.LastName} - {user.Email}!";
                this.messageService.AddMessageAtDB(userId, message);
                result.Add(message);
                returnModel.SelectedBook = selectedBook;
                returnModel.SelectedUser = selectedUser;
            }
            else
            {
                result.Add(chackInputData);
            }

            result.Add(returnModel);
            return result;
        }

        public List<object> EditingGivinBook(
        GiveBookViewModel model,
        string userId,
        string givenBookId,
        string selectedBookId,
        string selectedUserId)
        {
            var chackInputData = this.ChackInputData(selectedUserId, selectedBookId);
            List<object> result = new List<object>();
            var allUsers = this.userService.GetUsers(model.AllUsers);
            var allBooks = this.allBooksServices.GetBooks(model.AllBooks, userId);
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = allBooks,
                AllUsers = allUsers,
            };
            if (string.IsNullOrEmpty(chackInputData))
            {
                var selectedUser = this.SelectingUser(selectedUserId);
                var selectedBook = this.SelectingBook(selectedBookId);

                var getBook = this.context.GetBooks
                    .FirstOrDefault(gb => gb.Id == givenBookId);
                var book = this.context.Books.FirstOrDefault(b => b.Id == selectedBookId);
                var user = this.context.Users.FirstOrDefault(u => u.Id == selectedUserId);
                string message;
                if (getBook != null)
                {
                    getBook.Book = book;
                    getBook.BookId = selectedUserId;
                    getBook.User = user;
                    getBook.UserId = selectedUserId;
                    this.context.SaveChanges();
                    var library = this.context.Users.FirstOrDefault(u => u.Id == userId);
                    message = $"Успешно редактирана взета книга от {library.LibararyName} - {library.Email}!";
                    this.messageService.AddMessageAtDB(selectedUserId, message);

                    message = $"Успешно редактирана дадена книгана на {user.FirstName} {user.LastName} - {user.Email}!";
                    this.messageService.AddMessageAtDB(userId, message);
                }
                else
                {
                    message = "Изберете книга";
                }

                result.Add(message);
                returnModel.SelectedBook = selectedBook;
                returnModel.SelectedUser = selectedUser;
            }
            else
            {
                result.Add(chackInputData);
            }

            result.Add(returnModel);
            return result;
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

        private string ChackInputData(string selectedUserId, string selectedBookId)
        {
            StringBuilder result = new StringBuilder();
            if (string.IsNullOrEmpty(selectedUserId))
            {
                result.AppendLine("Моля изберете потребител");
            }

            if (string.IsNullOrEmpty(selectedBookId))
            {
                result.AppendLine("Моля изберете книга");
            }

            return result.ToString().Trim();
        }
    }
}
