namespace ELibrarySystem.Services.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ELibrarySystem.Data;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.AdminAccount;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.AdminAccount;
    using ELibrarySystem.Web.ViewModels.SharedResources;

    public class StatsAdminService : IStatsAdminService
    {
        private ApplicationDbContext context;

        private IMessageService messageService;

        public StatsAdminService(
            ApplicationDbContext context,
            IMessageService messageService)
        {
            this.context = context;
            this.messageService = messageService;
        }

        public StatsAdminViewModel PreparedPage(string userId)
        {
            var model = new StatsAdminViewModel();
            var returnModel = this.SearchStats(model, userId);
            return returnModel;
        }

        public StatsAdminViewModel SearchStats(StatsAdminViewModel model, string userId)
        {
            var searchUser = model.SearchUser;
            var returnModel = new StatsAdminViewModel()
            {
                SearchUser = searchUser,
                ChartAddedUsers = this.GetDataChartAddedUsers(searchUser),
                ChartAddedBookSinceSixМonth = this.ChartAddedBookSinceSixМonth(),
            };
            return returnModel;
        }

        private ChartAddedUsers GetDataChartAddedUsers(ApplicationUser searchUser)
        {
            var chartData = new List<ChartAddedUserData>();
            var groups = this.context.Users
              .Where(u =>
                    u.DeletedOn == null)
              .Select(u => new UserData()
              {
                  Type = u.Type,
                  CreatedOn = u.CreatedOn,
              })
              .GroupBy(u => u.CreatedOn.Year + " " + u.CreatedOn.Month)
              .Take(6)
              .ToList();

            foreach (var group in groups)
            {
                List<UserData> addedUsersOfMonth = group.Select(group => group).ToList();
                if (addedUsersOfMonth.Count > 0)
                {
                    var gb = addedUsersOfMonth[0];
                    string createdOnMonth = this.MonthToSring(gb.CreatedOn.Month);
                    int countAllUsers = addedUsersOfMonth.Count;
                    int countAdmins = addedUsersOfMonth.Where(u => u.Type == "admin").Count();
                    int countLibrarys = addedUsersOfMonth.Where(u => u.Type == "library").Count();
                    int countReaders = addedUsersOfMonth.Where(u => u.Type == "user").Count();
                    chartData.Add(new ChartAddedUserData(
                       createdOnMonth,
                       countAllUsers,
                       countAdmins,
                       countLibrarys,
                       countReaders));
                }
            }

            var chartAddedUsers = new ChartAddedUsers("Регистрирани потребители през последните 6 месеца", chartData);
            return chartAddedUsers;
        }

        private ChartViewModel ChartAddedBookSinceSixМonth()
        {
            var chartData = new List<ChartDataViewModel>();

            var groups = this.context.Books
                .Where(gb =>
                   gb.DeletedOn == null)
               .GroupBy(b => b.CreatedOn.Year + " " + b.CreatedOn.Month)
               .Take(6)
               .ToList();

            foreach (var group in groups)
            {
                List<Book> bookOfMonth = group.Select(group => group).ToList();
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
