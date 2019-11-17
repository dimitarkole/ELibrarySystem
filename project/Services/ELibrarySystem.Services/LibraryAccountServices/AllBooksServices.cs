namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public class AllBooksServices : IAllBooksServices
    {
        public ApplicationDbContext context;

        public IGenreService genreService;

        public IMessageService messageService;

        public AllBooksServices(
            ApplicationDbContext context,
            IGenreService genreService,
            IMessageService messageService)
        {
            this.context = context;
            this.genreService = genreService;
            this.messageService = messageService;
        }

        public AllBooksViewModel PreparedPage(string userId)
        {
            var model = new AllBooksViewModel();
            var returnModel = this.GetBooks(model, userId);
            return returnModel;
        }

        public AllBooksViewModel GetBooks(AllBooksViewModel model, string userId)
        {
            var bookName = model.SearchBook.BookName;
            var author = model.SearchBook.Author;
            var genreId = model.SearchBook.GenreId;
            var sortMethodId = model.SortMethodId;
            var countBooksOfPage = model.CountBooksOfPage;
            var currentPage = model.CurrentPage;

            var books = this.context.Books.Where(b =>
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

            books = this.SelectBooks(bookName, author, genreId, books);

            books = this.SortBooks(sortMethodId, books);

            var genres = this.genreService.GetAllGenres()
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

            var viewBook = books.Skip((currentPage - 1) * countBooksOfPage)
                                .Take(countBooksOfPage);
            var searchBook = new BookViewModel()
            {
                Author = author,
                BookName = bookName,
                GenreId = genreId,
            };

            var returnModel = new AllBooksViewModel()
            {
                Books = viewBook,
                SearchBook = searchBook,
                SortMethodId = sortMethodId,
                Genres = genres,
                MaxCountPage = maxCountPage,
                CurrentPage = currentPage,
                CountBooksOfPage = countBooksOfPage,
            };
            return returnModel;
        }

        public AllBooksViewModel DeleteBook(
           string userId,
           AllBooksViewModel model,
           string bookId)
        {
            var deleteBook = this.context.Books.FirstOrDefault(b => b.Id == bookId);
            if (deleteBook != null)
            {
                deleteBook.DeletedOn = DateTime.UtcNow;
                this.context.SaveChanges();
                string result = "Успешно премахната книганата!";
                this.messageService.AddMessageAtDB(userId, result);
            }

            var returnModel = this.GetBooks(model, userId);
            return returnModel;
        }

        public AllBooksViewModel ChangeActivePage(AllBooksViewModel model, string userId, int newPage)
        {
            model.CurrentPage = newPage;
            return this.GetBooks(model, userId);
        }

        private IQueryable<BookViewModel> SortBooks(
            string sortMethodId,
            IQueryable<BookViewModel> books)
        {
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

            return books;
        }

        private IQueryable<BookViewModel> SelectBooks(
          string bookName,
          string author,
          string genreId,
          IQueryable<BookViewModel> books)
        {
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

            return books;
        }
    }
}
