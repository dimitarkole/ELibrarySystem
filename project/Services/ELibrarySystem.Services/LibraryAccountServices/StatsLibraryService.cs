namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    using ELibrarySystem.Data;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.SharedResources;

    public class StatsLibraryService : IStatsLibraryService
    {
        private ApplicationDbContext context;

        private IGenreService genreService;

        private IMessageService messageService;

        public StatsLibraryService(
            ApplicationDbContext context,
            IGenreService genreService,
            IMessageService messageService)
        {
            this.context = context;
            this.genreService = genreService;
            this.messageService = messageService;
        }

        public StatsViewModel PreparedPage(string userId)
        {
            var model = new StatsViewModel();
            var returnModel = this.SearchStats(model, userId);
            return returnModel;
        }

        public StatsViewModel SearchStats(StatsViewModel model, string userId)
        {
            model.Genres = this.GetGenre();
            var searchBook = model.SearchBook;
            var returnModel = new StatsViewModel()
            {
                SearchBook = searchBook,
                Genres = model.Genres,
                ChartGettenBookSinceSixМonth = this.ChartGettenBookSinceSixМonth(searchBook, userId),
                ChartAddedBookSinceSixМonth = this.ChartAddedBookSinceSixМonth(searchBook, userId),
            };
            return returnModel;
        }

        private ChartViewModel ChartGettenBookSinceSixМonth(Book searchBook, string userId)
        {
            var chartData = new List<ChartDataViewModel>();
            var groups = this.context.GetBooks
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

                  FirstName = gb.User.FirstName,
                  LastName = gb.User.LastName,
                  UserName = gb.User.UserName,

                  ReturnedOn = gb.ReturnedOn,
                  CreatedOn = gb.CreatedOn,
              })
              .GroupBy(gb => gb.CreatedOn.Year + " " + gb.CreatedOn.Month)
              .Take(6)
              .ToList();

            string bookName = searchBook.BookName;
            string author = searchBook.Author;
            string genreId = searchBook.GenreId;

            foreach (var group in groups)
            {
                List<GivenBookViewModel> getBookOfMonth = group.Select(group => group).ToList();
                getBookOfMonth = this.SelectGettenBookOfMonthViewModel(bookName, author, genreId, getBookOfMonth);
                if (getBookOfMonth.Count > 0)
                {
                    var gb = getBookOfMonth[0];
                    string createdOnMonth = this.MonthToSring(gb.CreatedOn.Month);
                    chartData.Add(new ChartDataViewModel(
                       createdOnMonth,
                       getBookOfMonth.Count));
                }
            }

            var chartGettenBookSinceSixМonth = new ChartViewModel("Взети книги за последните 6 месеца", chartData);
            return chartGettenBookSinceSixМonth;
        }


        private ChartViewModel ChartAddedBookSinceSixМonth(Book searchBook, string userId)
        {
            var chartData = new List<ChartDataViewModel>();

            var groups = this.context.Books
                .Where(gb =>
                   gb.DeletedOn == null
                   && gb.UserId == userId)
               .GroupBy(b => b.CreatedOn.Year + " " + b.CreatedOn.Month)
               .Take(6)
               .ToList();

            string bookName = searchBook.BookName;
            string author = searchBook.Author;
            string genreId = searchBook.GenreId;

            foreach (var group in groups)
            {
                List<Book> bookOfMonth = group.Select(group => group).ToList();
                bookOfMonth = this.SelectBooksAddedBookOfMonthViewModel(bookName, author, genreId, bookOfMonth);
                if (bookOfMonth.Count > 0)
                {
                    var gb = bookOfMonth[0];
                    string createdOnMonth = this.MonthToSring(gb.CreatedOn.Month);

                    chartData.Add(new ChartDataViewModel(
                        createdOnMonth,
                        bookOfMonth.Count));
                }
            }

            var chartGettenBookSinceSixМonth = new ChartViewModel("Добавени книги за последните 6 месеца", chartData);
            return chartGettenBookSinceSixМonth;
        }

        private List<GenreListViewModel> GetGenre()
        {
            var genres = this.genreService.GetAllGenres()
                       .OrderByDescending(x => x.Name).ToList();

            var genre = new GenreListViewModel()
            {
                Id = null,
                Name = "Изберете жанр",
            };

            genres.Add(genre);
            genres.Reverse();
            return genres;
        }

        private List<Book> SelectBooksAddedBookOfMonthViewModel(
          string bookName,
          string author,
          string genreId,
          List<Book> books)
        {
            if (bookName != null)
            {
                books = books.Where(b => b.BookName.Contains(bookName)).ToList();
            }

            if (author != null)
            {
                books = books.Where(b => b.Author.Contains(author)).ToList();
            }

            if (genreId != null)
            {
                books = books.Where(b => b.GenreId == genreId).ToList();
            }

            return books;
        }

        private List<GivenBookViewModel> SelectGettenBookOfMonthViewModel(
            string bookName,
            string author,
            string genreId,
            List<GivenBookViewModel> books)
        {
            if (bookName != null)
            {
                books = books.Where(b => b.BookName.Contains(bookName)).ToList();
            }

            if (author != null)
            {
                books = books.Where(b => b.Author.Contains(author)).ToList();
            }

            if (genreId != null)
            {
                books = books.Where(b => b.GenreId == genreId).ToList();
            }

            return books;
        }

        private string MonthToSring(int month)
        {
            string result;
            switch (month)
            {
                case 1: result = "Януари"; break;
                case 2: result = "Февруари"; break;
                case 3: result = "Март"; break;
                case 4: result = "Април"; break;
                case 5: result = "Май"; break;
                case 6: result = "Юни"; break;
                case 7: result = "Юли"; break;
                case 8: result = "Август"; break;
                case 9: result = "Септември"; break;
                case 10: result = "Октомври"; break;
                case 11: result = "Ноември"; break;
                case 12: result = "Декември"; break;
                default:
                    result = "null";
                    break;
            }

            return result;
        }
    }
}
