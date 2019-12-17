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
            var catalogNumber = model.CatalogNumber;
            var commentar = model.Commentar;
            var genreObj = this.context.Genres.FirstOrDefault(g =>
                g.Id == genreId
                && g.DeletedOn == null);

            var bookCheker1 = this.context.Books.Where(b =>
                    b.BookName == bookName
                    && b.Author == author
                    && b.UserId == userId
                    && b.DeletedOn == null
                    && b.CatalogNumber.Equals(catalogNumber) == true)
                .ToList();
            if (bookCheker1.Count == 0)
            {
                var bookCheker2 = this.context.Books.FirstOrDefault(b =>
                    b.CatalogNumber == catalogNumber
                    && b.DeletedOn == null);
                string result = "Каталожният номер съвпада с каталожния номер на друга книга!";
                if (bookCheker2 == null)
                {
                    result = this.ChackeInputData(bookName, author, catalogNumber);
                    if (result == string.Empty)
                    {
                        var user = this.context.Users.FirstOrDefault(u => u.Id == userId);

                        var newBook = new Book()
                        {
                            BookName = bookName,
                            Author = author,
                            GenreId = genreId,
                            Genre = genreObj,
                            UserId = userId,
                            CatalogNumber = catalogNumber,
                            Commentar = commentar,
                            User = user,
                        };
                        this.context.Books.Add(newBook);
                        genreObj.Books.Add(newBook);
                        this.context.SaveChanges();
                        result = "Успешно добавена книганата!";
                        this.messageService.AddMessageAtDB(userId, result);
                    }
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
            var catalogNumber = model.CatalogNumber;
            var commentar = model.Commentar;
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

                var bookCheker1 = this.context.Books.Where(b =>
                       b.Id != bookId
                       && b.BookName == bookName
                       && b.Author == author
                       && b.UserId == userId
                       && b.CatalogNumber.Equals(catalogNumber) == true
                       && b.DeletedOn == null)
                   .ToList();
                if (bookCheker1.Count == 0)
                {
                    var checkDublicateBook = this.context.Books.FirstOrDefault(b =>
                       b.Id != bookId
                       && b.CatalogNumber == catalogNumber
                       && b.DeletedOn == null);

                    if (checkDublicateBook == null)
                    {
                        resultTitle = this.ChackeInputData(bookName, author, catalogNumber);
                        if (resultTitle == string.Empty)
                        {
                            book.BookName = bookName;
                            book.Author = author;
                            book.GenreId = genreId;
                            book.Genre = genreObj;
                            book.UserId = userId;
                            book.CatalogNumber = catalogNumber;
                            book.Commentar = commentar;

                            genreObj.Books.Add(book);
                            this.context.SaveChanges();
                            resultTitle = "Успешно редактирана книгана!";
                            this.messageService.AddMessageAtDB(userId, resultTitle);
                        }
                    }
                    else
                    {
                        resultTitle = "Каталожният номер съвпада с каталожния номер на друга книга!";
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
                CatalogNumber = book.CatalogNumber,
                Commentar = book.Commentar,
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

        private string ChackeInputData(string bookName, string author, string catalogNumber)
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

            if (string.IsNullOrEmpty(catalogNumber)
              || string.IsNullOrWhiteSpace(catalogNumber)
              || author.Length < 3)
            {
                errors.AppendLine("Каталожният номер трябва да има поне 3 символа!");
            }

            return errors.ToString().Trim();
        }
    }
}
