namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    using ELibrarySystem.Data;
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
            var model = new StatsViewModel()
            {
                ChartGettenBookSinceSixМonth = this.ChartGettenBookSinceSixМonth(),
            };
            return model;
        }

        private ChartViewModel ChartGettenBookSinceSixМonth()
        {
            var chartData = new List<ChartDataViewModel>();
            DateTime dataType = DateTime.UtcNow;
            for (int i = 0; i < 6; i++)
            {
                var month = dataType.Month;
                var countGetBookOfMonth = this.context.GetBooks
                    .Where(gb =>
                        gb.DeletedOn == null
                        && gb.CreatedOn.Year == dataType.Year
                        && gb.CreatedOn.Month == dataType.Month)
                    .Count();

                chartData.Add(new ChartDataViewModel
                {
                    DimensionOne = this.MonthToSring(month),
                    Quantity = countGetBookOfMonth,
                });
                month--;
            }

            var chartGettenBookSinceSixМonth = new ChartViewModel()
            {
                ChartData = chartData,
            };

            return chartGettenBookSinceSixМonth;
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
