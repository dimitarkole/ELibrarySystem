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
        public ApplicationDbContext context;
        public IGenreService genreService;

        public IMessageService messageService;

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
            var bookName = model.BookName;
            var author = model.Author;
            var genreId = model.GenreId;
            var sortMethodId = model.SortMethodId;
            var countBooksOfPage = model.CountBooksOfPage;
            var currentPage = model.CurrentPage;

            var getbooks = this.context.GetBooks.Where(b =>
                b.DeletedOn == null
                && b.UserId == userId)
                .Select(b => new TakenBookViewModel()
                {
                    Author = b.Book.Author,
                    GetBookId = b.Id,
                    BookName = b.Book.BookName,
                    GenreName = b.Book.Genre.Name,
                    GenreId = b.Book.GenreId,
                    Returned = b.ReturnedOn,
                });

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
                getbooks = getbooks.Where(b => b.GenreId == genreId);
            }

            if (sortMethodId == "Име на книгата я-а")
            {
                getbooks = getbooks.OrderByDescending(b => b.BookName);
            }
            else if (sortMethodId == "Име на автора а-я")
            {
                getbooks = getbooks.OrderBy(b => b.Author);
            }
            else if (sortMethodId == "Име на автора я-а")
            {
                getbooks = getbooks.OrderByDescending(b => b.Author);
            }
            else if (sortMethodId == "Жанр а-я")
            {
                getbooks = getbooks.OrderBy(b => b.GenreName);
            }
            else if (sortMethodId == "Жанр я-а")
            {
                getbooks = getbooks.OrderByDescending(b => b.GenreName);
            }
            else
            {
                getbooks = getbooks.OrderBy(b => b.BookName);
            }

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
            var returnModel = new TakenBooksViewModel()
            {
                Books = getbooks,
                Author = author,
                BookName = bookName,
                GenreId = genreId,
                SortMethodId = sortMethodId,
                Genres = genres,
                MaxCountPage = maxCountPage,
                CurrentPage = currentPage,
                CountBooksOfPage = countBooksOfPage,
            };
            return returnModel;
        }
    }
}