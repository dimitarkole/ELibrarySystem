namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public class GetAllBooksServices : IGetAllBooksServices
    {
        public ApplicationDbContext context;

        public IGenreService genreService;

        public GetAllBooksServices(ApplicationDbContext context,
            IGenreService genreService)
        {
            this.context = context;
            this.genreService = genreService;
        }

        public AllBooksViewModel PreparedPage(string userId)
        {
            var model = new AllBooksViewModel();
            var returnModel = this.GetBooks(model, userId);
            return returnModel;
        }

        private AllBooksViewModel GetBooks(AllBooksViewModel model, string userId)
        {
            var bookName = model.BookName;
            var author = model.Author;
            var genreId = model.GenreId;
            var sortMethodId = model.SortMethodId;
            var countBooksOfPage = model.CountBooksOfPage;
            var currentPage = model.CurrentPage;


            var books = context.Books.Where(b =>
              b.DeletedOn == null
              && b.UserId == userId)
              .Select(b => new BookViewModel()
              {
                  Author = b.Author,
                  BookId = b.Id,
                  BookName = b.BookName,
                  GenreName = b.Genre.Name,
                  GenreId = b.GenreId,
              });

            if (bookName != null)
            {
                books = books.Where(b => b.BookName.Contains(bookName));
            }

            if (author != null)
            {
                books = books.Where(b => b.Author.Contains(author));
            }

            if (genreId != null)
            {
                books = books.Where(b => b.GenreId == genreId);
            }

            if (sortMethodId == "Име на книгата я-а")
            {
                books = books.OrderByDescending(b => b.BookName);
            }
            else if (sortMethodId == "Име на автора а-я")
            {
                books = books.OrderBy(b => b.Author);
            }
            else if (sortMethodId == "Име на автора я-а")
            {
                books = books.OrderByDescending(b => b.Author);
            }
            else if (sortMethodId == "Жанр а-я")
            {
                books = books.OrderBy(b => b.GenreName);
            }
            else if (sortMethodId == "Жанр я-а")
            {
                books = books.OrderByDescending(b => b.GenreName);
            }
            else
            {
                books = books.OrderBy(b => b.BookName);
            }

            var genres = genreService.GetAllGenres()
                 .OrderByDescending(x => x.Name).ToList();

            var genre = new GenreListViewModel()
            {
                Id = null,
                Name = "Изберете жанр",
            };

            genres.Add(genre);
            genres.Reverse();
            int maxCountPage = books.Count() / countBooksOfPage;
            if (books.Count() % countBooksOfPage != 0)
            {
                maxCountPage++;
            }

            var viewBook = books.Skip((countBooksOfPage - 1) * countBooksOfPage)
                                .Take(countBooksOfPage);

            var returnModel = new AllBooksViewModel()
            {
                Books = viewBook,
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

        AllBooksViewModel IGetAllBooksServices.GetBooks(AllBooksViewModel model, string userId)
        {
            return this.GetBooks(model, userId);
        }
    }
}
