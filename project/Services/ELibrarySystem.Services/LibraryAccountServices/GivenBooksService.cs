﻿namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public class GivenBooksService : IGivenBooksService
    {
        private ApplicationDbContext context;

        private IGenreService genreService;

        private IMessageService messageService;


        public GivenBooksService(
            ApplicationDbContext context,
            IGenreService genreService,
            IMessageService messageService)
        {
            this.context = context;
            this.genreService = genreService;
            this.messageService = messageService;
        }

        public GivenBooksViewModel ChangeActivePage(
            GivenBooksViewModel model,
            string userId,
            int newPage)
        {
            model.CurrentPage = newPage;
            return this.GetGevenBooks(model, userId);
        }

        public List<object> DeletingBook(GivenBooksViewModel model, string userId, string givenBookId)
        {
            var givenBook = this.context.GetBooks
             .FirstOrDefault(gb => gb.Id == givenBookId);
            List<object> result = new List<object>();
            result.Add(this.GetGevenBooks(model, userId));

            if (givenBook != null)
            {
                givenBook.DeletedOn = DateTime.UtcNow;
                givenBook.ReturnedOn = DateTime.UtcNow;

                this.context.SaveChanges();

                result.Add("Успершно изтриване на взета книгата!");
            }
            else
            {
                result.Add("Няма дадена книга на този потребител!");
            }

            return result;
        }

        public GivenBooksViewModel GetGevenBooks(GivenBooksViewModel model, string userId)
        {
            var firstName = model.SearchGivenBook.FirstName;
            var lastName = model.SearchGivenBook.LastName;
            var email = model.SearchGivenBook.Email;
            var bookName = model.SearchGivenBook.BookName;
            var author = model.SearchGivenBook.Author;
            var genreId = model.SearchGivenBook.GenreId;
            var sortMethodId = model.SortMethodId;
            var countBooksOfPage = model.CountBooksOfPage;
            var currentPage = model.CurrentPage;
            var catalogNumber = model.SearchGivenBook.CatalogNumber;

            var givenBooks = this.context.GetBooks
                .Where(gb =>
                    gb.DeletedOn == null
                    && gb.Book.UserId == userId)
                .Select(gb => new GivenBookViewModel()
                {
                    Author = gb.Book.Author,
                    Id = gb.Id,
                    BookName = gb.Book.BookName,
                    GenreName = gb.Book.Genre.Name,
                    GenreId = gb.Book.GenreId,
                    CatalogNumber = gb.Book.CatalogNumber,
                    FirstName = gb.User.FirstName,
                    LastName = gb.User.LastName,
                    UserName = gb.User.UserName,

                    ReturnedOn = gb.ReturnedOn,
                    CreatedOn = gb.CreatedOn,
                });

            givenBooks = this.SelectBooks(
                bookName,
                author,
                genreId,
                firstName,
                lastName,
                email,
                catalogNumber,
                givenBooks);

            givenBooks = this.SortBooks(sortMethodId, givenBooks);

            var genres = this.genreService.GetAllGenres()
                 .OrderByDescending(x => x.Name).ToList();

            var genre = new GenreListViewModel()
            {
                Id = null,
                Name = "Изберете жанр",
            };

            genres.Add(genre);
            genres.Reverse();
            int maxCountPage = givenBooks.Count() / countBooksOfPage;
            if (givenBooks.Count() % countBooksOfPage != 0)
            {
                maxCountPage++;
            }

            var viewBook = givenBooks.Skip((currentPage - 1) * countBooksOfPage)
                                .Take(countBooksOfPage);

            var searchGivenBook = new GivenBookViewModel()
            {
                CatalogNumber = catalogNumber,
                Author = author,
                BookName = bookName,
                GenreId = genreId,
            };

            var returnModel = new GivenBooksViewModel()
            {
                Books = viewBook,
                SearchGivenBook = searchGivenBook,
                SortMethodId = sortMethodId,
                Genres = genres,
                MaxCountPage = maxCountPage,
                CurrentPage = currentPage,
                CountBooksOfPage = countBooksOfPage,
            };
            return returnModel;
        }

        public GivenBooksViewModel PreparedPage(string userId)
        {
            var model = new GivenBooksViewModel();
            var returnModel = this.GetGevenBooks(model, userId);
            return returnModel;
        }

        public List<object> ReturningBook(
            GivenBooksViewModel model,
            string userId,
            string givenBookId)
        {
            var givenBook = this.context.GetBooks
                .FirstOrDefault(gb => gb.Id == givenBookId);
            List<object> result = new List<object>();
            result.Add(this.GetGevenBooks(model, userId));

            if (givenBook != null)
            {
                givenBook.ReturnedOn = DateTime.UtcNow;
                this.context.SaveChanges();
                var message = $"Успершно връщане на книгата!";

                result.Add(message);
                this.messageService.AddMessageAtDB(userId, message);
                this.messageService.AddMessageAtDB(givenBook.UserId, message);
            }
            else
            {
                result.Add("Няма дадена книга на този потребител!");
            }

            return result;
        }

        public List<object> SendMessageForReturningBook(
           GivenBooksViewModel model,
           string userId,
           string givenBookId)
        {
            var givenBook = this.context.GetBooks
                .FirstOrDefault(gb => gb.Id == givenBookId);
            List<object> result = new List<object>();
            result.Add(this.GetGevenBooks(new GivenBooksViewModel(), userId));

            if (givenBook != null)
            {
                var book = this.context.Books.FirstOrDefault(b => b.Id == givenBook.BookId);

                var message = $"Напомняне за връщане на книгата - {book.BookName} {book.Author}!";
                this.messageService.AddMessageAtDB(givenBook.UserId, message);

                result.Add("Успершно изпратено напомняне за връщане на книга!");
            }
            else
            {
                result.Add("Не успершно изпратено напомняне за връщане на книга!");
            }

            return result;
        }

        private IQueryable<GivenBookViewModel> SelectBooks(
          string bookName,
          string author,
          string genreId,
          string firstName,
          string lastName,
          string email,
          string catalogNumber,
          IQueryable<GivenBookViewModel> givenBooks)
        {
            if (catalogNumber != null)
            {
                givenBooks = givenBooks.Where(b => b.CatalogNumber.Contains(catalogNumber));
            }

            if (bookName != null)
            {
                givenBooks = givenBooks.Where(b => b.BookName.Contains(bookName));
            }

            if (author != null)
            {
                givenBooks = givenBooks.Where(b => b.Author.Contains(author));
            }

            if (genreId != null)
            {
                givenBooks = givenBooks.Where(b => b.GenreId == genreId);
            }

            if (firstName != null)
            {
                givenBooks = givenBooks.Where(b => b.FirstName.Contains(firstName));
            }

            if (lastName != null)
            {
                givenBooks = givenBooks.Where(b => b.LastName.Contains(lastName));
            }

            if (email != null)
            {
                givenBooks = givenBooks.Where(b => b.Email.Contains(email));
            }

            return givenBooks;
        }

        private IQueryable<GivenBookViewModel> SortBooks(
           string sortMethodId,
           IQueryable<GivenBookViewModel> givenBooks)
        {
            if (sortMethodId == "Име на книгата я-а")
            {
                givenBooks = givenBooks.OrderByDescending(x => x.ReturnedOn)
                    .ThenByDescending(b => b.BookName);
            }
            else if (sortMethodId == "Име на автора а-я")
            {
                givenBooks = givenBooks.OrderByDescending(x => x.ReturnedOn)
                    .ThenBy(b => b.Author);
            }
            else if (sortMethodId == "Име на автора я-а")
            {
                givenBooks = givenBooks.OrderByDescending(x => x.ReturnedOn)
                    .ThenByDescending(b => b.Author);
            }
            else if (sortMethodId == "Жанр а-я")
            {
                givenBooks = givenBooks.OrderByDescending(x => x.ReturnedOn)
                    .ThenBy(b => b.GenreName);
            }
            else if (sortMethodId == "Жанр я-а")
            {
                givenBooks = givenBooks.OrderByDescending(x => x.ReturnedOn)
                    .ThenByDescending(b => b.GenreName);
            }
            else
            {
                givenBooks = givenBooks.OrderByDescending(x => x.ReturnedOn)
                    .ThenBy(b => b.BookName);
            }

            return givenBooks;
        }
    }
}
