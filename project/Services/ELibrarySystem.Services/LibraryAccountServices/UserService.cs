namespace ELibrarySystem.Services.LibraryAccountServices
{
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UserService : IUserService
    {
        public ApplicationDbContext context;

        public IGenreService genreService;

        public IMessageService messageService;

        public UserService(ApplicationDbContext context,
            IGenreService genreService,
            IMessageService messageService)
        {
            this.context = context;
            this.genreService = genreService;
            this.messageService = messageService;
        }

        public AllUsersViewModel PreparedPage()
        {
            var model = new AllUsersViewModel();
            var returnModel = this.GetUsers(model);
            return returnModel;
        }

        private AllUsersViewModel GetUsers(AllUsersViewModel model)
        {
            var userName = model.UserName;
            var firstName = model.FirstName;
            var lastName = model.LastName;
            var sortMethodId = model.SortMethodId;
            var currentPage = model.CurrentPage;
            var countUsersOfPage = model.CountUsersOfPage;

            var users = this.context.Users
                .Where(u => u.Type == "user")
                .Select(u => new UserViewModel()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserId = u.Id,
                    UserName = u.UserName,
                });

            if (userName != null)
            {
                users = users.Where(u => u.UserName.Contains(userName));
            }

            if (firstName != null)
            {
                users = users.Where(u => u.FirstName.Contains(firstName));
            }

            if (lastName != null)
            {
                users = users.Where(u => u.LastName.Contains(lastName));
            }


            if (sortMethodId == "Потребителско име а-я")
            {
                users = users.OrderBy(u => u.UserName);
            }
            else if (sortMethodId == "Потребителско име я-а")
            {
                users = users.OrderByDescending(u => u.UserName);
            }

            int maxCountPage = users.Count() / countUsersOfPage;
            if (users.Count() % countUsersOfPage != 0)
            {
                maxCountPage++;
            }

            var viewUsers = users.Skip((currentPage - 1) * countUsersOfPage)
                                .Take(countUsersOfPage);

            var returnModel = new AllUsersViewModel()
            {
                Users = viewUsers,
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                SortMethodId = sortMethodId,
                MaxCountPage = maxCountPage,
                CurrentPage = currentPage,
                CountUsersOfPage = countUsersOfPage,
            };
            return returnModel;
        }
    }
}
