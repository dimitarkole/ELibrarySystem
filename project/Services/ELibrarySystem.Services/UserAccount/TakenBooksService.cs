﻿namespace ELibrarySystem.Services.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Services.Contracts.UserAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.UserAccount;

    public class TakenBooksService : ITakenBooksService
    {
        private ApplicationDbContext context;

        private IGenreService genreService;

        private IMessageService messageService;

        public TakenBooksService(ApplicationDbContext context,
            IGenreService genreService,
            IMessageService messageService)
        {
            this.context = context;
            this.genreService = genreService;
            this.messageService = messageService;
        }

        public TakenBooksViewModel ChangeActivePage(TakenBooksViewModel model, string userId, int newPage)
        {
            model.CurrentPage = newPage;
            return this.GetBooks(model, userId);
        }

        public TakenBooksViewModel PreparedPage(string userId)
        {
            var model = new TakenBooksViewModel();
            var returnModel = this.GetBooks(model, userId);
            return returnModel;
        }

        public TakenBooksViewModel TakenBooks(TakenBooksViewModel model, string userId)
        {
            var returnModel = this.GetBooks(model, userId);
            return returnModel;
        }

        private TakenBooksViewModel GetBooks(TakenBooksViewModel model, string userId)
        {
            var bookName = model.SearchTakenBook.BookName;
            var author = model.SearchTakenBook.Author;
            var genreId = model.SearchTakenBook.GenreId;
            var sortMethodId = model.SortMethodId;
            var countBooksOfPage = model.CountBooksOfPage;
            var currentPage = model.CurrentPage;
            var catalogNumber = model.SearchTakenBook.CatalogNumber;


            var getbooks = this.context.GetBooks.Where(b =>
                b.DeletedOn == null
                && b.UserId == userId)
                .Select(b => new TakenBookViewModel()
                {
                    Author = b.Book.Author,
                    Id = b.Id,
                    BookName = b.Book.BookName,
                    Genre = b.Book.Genre.Name,
                    GenreId = b.Book.GenreId,
                    CreatedOn = b.CreatedOn,
                    ReturnedOn = b.ReturnedOn,
                    LibraryId = b.Book.UserId,
                    LibraryEmail = b.Book.User.Email,
                    CatalogNumber = b.Book.CatalogNumber,
                });

            getbooks = this.SelectBooks(catalogNumber,bookName, author, genreId, getbooks);

            getbooks = this.SortBooks(sortMethodId, getbooks);

            var genres = this.genreService.GetAllGenres()
                 .OrderByDescending(x => x.Name).ToList();

            var genre = new GenreListViewModel()
            {
                Id = null,
                Name = "Изберете жанр",
            };

            genres.Add(genre);
            genres.Reverse();
            int maxCountPage = getbooks.Count() / countBooksOfPage;
            if (getbooks.Count() % countBooksOfPage != 0)
            {
                maxCountPage++;
            }

            var viewBook = getbooks.Skip((currentPage - 1) * countBooksOfPage)
                                .Take(countBooksOfPage);
            var searchTakenBook = new TakenBookViewModel()
            {
                Author = author,
                BookName = bookName,
                GenreId = genreId,
            };

            var returnModel = new TakenBooksViewModel()
            {
                Books = getbooks,
                SearchTakenBook = searchTakenBook,
                SortMethodId = sortMethodId,
                Genres = genres,
                MaxCountPage = maxCountPage,
                CurrentPage = currentPage,
                CountBooksOfPage = countBooksOfPage,
            };
            return returnModel;
        }

        private IQueryable<TakenBookViewModel> SelectBooks(
         string catalogNumber,
         string bookName,
         string author,
         string genreId,
         IQueryable<TakenBookViewModel> getbooks)
        {
            if (catalogNumber != null)
            {
                getbooks = getbooks.Where(b => b.CatalogNumber.Contains(catalogNumber));
            }

            if (bookName != null)
            {
                getbooks = getbooks.Where(b => b.BookName.Contains(bookName));
            }

            if (author != null)
            {
                getbooks = getbooks.Where(b => b.Author.Contains(author));
            }

            if (genreId != null)
            {
                getbooks = getbooks.Where(b => b.Genre == genreId);
            }

            return getbooks;
        }

        private IQueryable<TakenBookViewModel> SortBooks(
         string sortMethodId,
         IQueryable<TakenBookViewModel> getbooks)
        {
            if (sortMethodId == "Име на книгата я-а")
            {
                getbooks = getbooks.OrderByDescending(b => b.ReturnedOn)
                    .ThenByDescending(b => b.BookName);
            }
            else if (sortMethodId == "Име на автора а-я")
            {
                getbooks = getbooks.OrderByDescending(b => b.ReturnedOn)
                    .ThenBy(b => b.Author);
            }
            else if (sortMethodId == "Име на автора я-а")
            {
                getbooks = getbooks.OrderByDescending(b => b.ReturnedOn)
                    .ThenByDescending(b => b.Author);
            }
            else if (sortMethodId == "Жанр а-я")
            {
                getbooks = getbooks.OrderByDescending(b => b.ReturnedOn)
                    .ThenBy(b => b.Genre);
            }
            else if (sortMethodId == "Жанр я-а")
            {
                getbooks = getbooks.OrderByDescending(b => b.ReturnedOn)
                    .ThenByDescending(b => b.Genre);
            }
            else
            {
                getbooks = getbooks.OrderByDescending(b => b.ReturnedOn)
                    .ThenBy(b => b.BookName);
            }

            return getbooks;
        }
    }
}
