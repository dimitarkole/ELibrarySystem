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

            var searchBook = model.SearchBook;
            var returnModel = new StatsViewModel()
            {
                SearchBook = searchBook,
                ChartGettenBookSinceSixМonth = this.ChartGettenBookSinceSixМonth(searchBook, userId),
                ChartAddedBookSinceSixМonth = this.ChartAddedBookSinceSixМonth(searchBook, userId),
            };
            return returnModel;
        }

        private ChartViewModel ChartGettenBookSinceSixМonth(Book searchBook, string userId)
        {
            var chartData = new List<ChartDataViewModel>();
            DateTime dataType = DateTime.UtcNow;
            IEnumerable<IGrouping<string, GettenBookOfMonthViewModel>> groups = this.context.GetBooks
                .Where(gb =>
                    gb.DeletedOn == null
                    && gb.Book.UserId == userId)
                .Select(gb => new GettenBookOfMonthViewModel(gb))
                .GroupBy(gb => gb.CreatedOnYearAndMonth)
                .Take(6);

            foreach (var group in groups)
            {
                List<GettenBookOfMonthViewModel> getBookOfMonth = group.Select(group => group).ToList();

                chartData.Add(new ChartDataViewModel(
                    getBookOfMonth[0].CreatedOnMonth,
                    getBookOfMonth.Count));
            }

            var chartGettenBookSinceSixМonth = new ChartViewModel("Взети книги за последните 6 месеца", chartData);
            return chartGettenBookSinceSixМonth;
        }

        private ChartViewModel ChartAddedBookSinceSixМonth(Book searchBook, string userId)
        {
            var chartData = new List<ChartDataViewModel>();
            DateTime dataType = DateTime.UtcNow;
            IEnumerable<IGrouping<string, AddedBookOfMonthViewModel>> groups = this.context.Books
                .Where(b =>
                    b.DeletedOn == null
                    && b.UserId == userId)
                .Select(b => new AddedBookOfMonthViewModel(b))
                .GroupBy(b => b.CreatedOnYearAndMonth)
                .Take(6);

            foreach (var group in groups)
            {
                List<AddedBookOfMonthViewModel> getBookOfMonth = group.Select(group => group).ToList();
                chartData.Add(new ChartDataViewModel(
                    getBookOfMonth[0].CreatedOnMonth,
                    getBookOfMonth.Count));
            }

            var chartGettenBookSinceSixМonth = new ChartViewModel("Добавени книги за последните 6 месеца", chartData);
            return chartGettenBookSinceSixМonth;
        }
    }
}
