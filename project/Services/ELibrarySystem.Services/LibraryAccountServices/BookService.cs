namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;

    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using ELibrarySystem.Data.Models;

    public class BookService : IBookService
    {
        private ApplicationDbContext context;

        private IGenreService genreService;

        private IMessageService messageService;

        public BookService(ApplicationDbContext context,
            IGenreService genreService,
            IMessageService messageService)
        {
            this.context = context;
            this.genreService = genreService;
            this.messageService = messageService;
        }

        public string AddBook(AddBookViewModel model, string userId)
        {
            var genreId = model.GenreId;
            var bookName = model.BookName;
            var author = model.Author;

            var genreObj = this.context.Genres.FirstOrDefault(g =>
                g.Id == genreId
                && g.DeletedOn == null);

            var book = this.context.Books.FirstOrDefault(b =>
                b.BookName == bookName
                && b.Author == author
                && b.UserId == userId
                && b.DeletedOn == null);
            var user = this.context.Users.FirstOrDefault(u => u.Id == userId);
            if (book == null)
            {
                string result = this.ChackeInputData(bookName, author);
                if (result == string.Empty)
                {
                    var newBook = new Book()
                    {
                        BookName = bookName,
                        Author = author,
                        GenreId = genreId,
                        Genre = genreObj,
                        UserId = userId,
                    };
                    this.context.Books.Add(newBook);
                    genreObj.Books.Add(newBook);
                    this.context.SaveChanges();
                    result = "Успешно добавена книганата!";
                    this.messageService.AddMessageAtDB(userId, result);
                }

                return result;
            }

            return "Книганата същесвува в библиотеката Ви!";
        }

        public List<object> EditBook(AddBookViewModel model, string userId)
        {
            var genreId = model.GenreId;
            var bookId = model.BookId;
            var bookName = model.BookName;
            var author = model.Author;
            var genreObj = this.context.Genres.FirstOrDefault(g =>
               g.Id == genreId
               && g.DeletedOn == null);
            var book = this.context.Books.FirstOrDefault(b => b.Id == bookId);
            model.Genres = this.genreService.GetAllGenres();
            model.BookId = bookId;
            var result = new List<object>();
            result.Add(model);
            string resultTitle;
            if (book != null)
            {
                var checkDublicateBook = this.context.Books.FirstOrDefault(b =>
                     b.Id != bookId
                     && b.BookName == bookName
                     && b.Author == author
                     && b.GenreId == genreId);
                if (checkDublicateBook == null)
                {
                    resultTitle = this.ChackeInputData(bookName, author);
                    if (resultTitle == string.Empty)
                    {
                        book.BookName = bookName;
                        book.Author = author;
                        book.GenreId = genreId;
                        book.Genre = genreObj;
                        book.UserId = userId;
                        genreObj.Books.Add(book);
                        this.context.SaveChanges();
                        resultTitle = "Успешно редактирана книгана!";
                        this.messageService.AddMessageAtDB(userId, resultTitle);
                    }
                }
                else
                {
                    resultTitle = "Редакцията на книгата дублира друга книга!";
                }
            }
            else
            {
                resultTitle = "Книганата не същесвува в библиотеката Ви!";
            }

            result.Add(resultTitle);
            return result;
        }

        public AddBookViewModel GetBookDataById(string bookId)
        {
            var book = this.context.Books.FirstOrDefault(b => b.Id == bookId);
            var genres = this.genreService.GetAllGenres();
            var model = new AddBookViewModel()
            {
                BookId = bookId,
                Author = book.Author,
                BookName = book.BookName,
                GenreId = book.GenreId,
                Genres = genres,
            };
            return model;
        }

        public AddBookViewModel PreparedAddBookPage()
        {
            var genres = this.genreService.GetAllGenres();

            var model = new AddBookViewModel()
            {
                Genres = genres,
            };
            return model;
        }

        private string ChackeInputData(string bookName, string author)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(bookName)
                || string.IsNullOrWhiteSpace(bookName)
                || bookName.Length < 5)
            {
                errors.AppendLine("Името на книгата трябва да има поне 5 символа!");
            }

            if (string.IsNullOrEmpty(author)
                || string.IsNullOrWhiteSpace(author)
                || author.Length < 5)
            {
                errors.AppendLine("Името на автора трябва да има поне 5 символа!");
            }

            return errors.ToString().Trim();
        }
    }
}
