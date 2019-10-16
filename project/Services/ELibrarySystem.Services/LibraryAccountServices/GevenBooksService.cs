namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public class GevenBooksService : IGevenBooksService
    {
        public ApplicationDbContext context;

        public IGenreService genreService;

        public GevenBooksService(ApplicationDbContext context,
            IGenreService genreService)
        {
            this.context = context;
            this.genreService = genreService;
        }

        public GivenBooksViewModel ChangeActivePage(GivenBooksViewModel model, string userId, int newPage)
        {
            model.CurrentPage = newPage;
            return this.GetGevenBooks(model, userId);
        }

        public GivenBooksViewModel GetGevenBooks(GivenBooksViewModel model, string userId)
        {
            var userName = model.UserName;
            var firstName = model.FirstName;
            var lastName = model.LastName;
            var email = model.Email;
            var bookName = model.BookName;
            var author = model.Author;
            var genreId = model.GenreId;
            var sortMethodId = model.SortMethodId;
            var countBooksOfPage = model.CountBooksOfPage;
            var currentPage = model.CurrentPage;

            var givenBooks = this.context.GetBooks.Where(gb =>
              gb.DeletedOn == null
              && gb.Book.UserId == userId)
              .Select(gb => new GivenBookViewModel()
              {
                  BookAuthor = gb.Book.Author,
                  Id = gb.Book.Id,
                  BookName = gb.Book.BookName,
                  BookGenre = gb.Book.Genre.Name,
                  BookGenreId = gb.Book.GenreId,

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
                userName,
                firstName,
                lastName,
                email,
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

            var returnModel = new GivenBooksViewModel()
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

        public GivenBooksViewModel PreparedPage(string userId)
        {
            var model = new GivenBooksViewModel();
            var returnModel = this.GetGevenBooks(model, userId);
            return returnModel;
        }

        public List<object> ReturnBook(string userId, string givenBookId)
        {
            var givenBook = this.context.GetBooks
                .FirstOrDefault(gb => gb.Id == givenBookId);
            List<object> result = new List<object>();
            result.Add(this.PreparedPage(userId));

            if (givenBook != null)
            {
                givenBook.ReturnedOn = DateTime.UtcNow;
                this.context.SaveChanges();
                result.Add("Успершно връщане на книгата!");
            }
            else
            {
                result.Add("Няма дадена книга на този потребител!");
            }

            return result;
        }

        private IQueryable<GivenBookViewModel> SelectBooks(
          string bookName,
          string author,
          string genreId,
          string userName,
          string firstName,
          string lastName,
          string email,
          IQueryable<GivenBookViewModel> givenBooks)
        {
            if (bookName != null)
            {
                givenBooks = givenBooks.Where(b => b.BookName.Contains(bookName));
            }

            if (givenBooks != null)
            {
                givenBooks = givenBooks.Where(b => b.BookAuthor.Contains(author));
            }

            if (genreId != null)
            {
                givenBooks = givenBooks.Where(b => b.BookGenre == genreId);
            }


            if (userName != null)
            {
                givenBooks = givenBooks.Where(b => b.UserName.Contains(userName));
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
                    .ThenBy(b => b.BookAuthor);
            }
            else if (sortMethodId == "Име на автора я-а")
            {
                givenBooks = givenBooks.OrderByDescending(x => x.ReturnedOn)
                    .ThenByDescending(b => b.BookAuthor);
            }
            else if (sortMethodId == "Жанр а-я")
            {
                givenBooks = givenBooks.OrderByDescending(x => x.ReturnedOn)
                    .ThenBy(b => b.BookGenre);
            }
            else if (sortMethodId == "Жанр я-а")
            {
                givenBooks = givenBooks.OrderByDescending(x => x.ReturnedOn)
                    .ThenByDescending(b => b.BookGenre);
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
