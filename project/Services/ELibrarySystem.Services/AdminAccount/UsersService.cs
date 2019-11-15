namespace ELibrarySystem.Services.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.AdminAccount;
    using ELibrarySystem.Web.ViewModels.AdminAccount;

    public class UsersService : IUsersService
    {
        public ApplicationDbContext context;

        public UsersService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public UsersViewModel PreparedPage(string userId)
        {
            var model = new UsersViewModel();
            var returnModel = this.GetUsers(model, userId);
            return returnModel;
        }

        public UsersViewModel GetUsers(UsersViewModel model, string userId)
        {
            var email = model.Email;
            var firstName = model.FirstName;
            var lastName = model.LastName;
            var libraryName = model.LibraryName;

            var sortMethodId = model.SortMethodId;
            var countUsersOfPage = model.CountUsersOfPage;
            var currentPage = model.CurrentPage;

            var users = this.context.Users.Where(u =>
              u.DeletedOn == null)
              .Select(u => new UserViewModel()
              {
                  Email = u.Email,
                  FirstName = u.FirstName,
                  LastName = u.LastName,
                  UserId = u.Id,
                  LibraryName = u.LibararyName,
              });

            users = this.SelectUsers(
                users,
                email,
                firstName,
                lastName,
                libraryName);

            users = this.SortBooks(sortMethodId, users);
            int maxCountPage = users.Count() / countUsersOfPage;
            if (users.Count() % countUsersOfPage != 0)
            {
                maxCountPage++;
            }

            var viewUsers = users.Skip((currentPage - 1) * countUsersOfPage)
                                .Take(countUsersOfPage);

            var returnModel = new UsersViewModel()
            {
                Email = email,
                FirstName = firstName,
                CountUsersOfPage = countUsersOfPage,
                LastName = lastName,
                LibraryName = libraryName,
                SortMethodId = sortMethodId,
                MaxCountPage = maxCountPage,
                Users = viewUsers,
                CurrentPage = currentPage,
            };
            return returnModel;
        }

        private IQueryable<UserViewModel> SortBooks(string sortMethodId, IQueryable<UserViewModel> users)
        {
            if (sortMethodId == "Email на потребителя а-я")
            {
                users = users.OrderBy(u => u.Email);
            }
            else if (sortMethodId == "Email на потребителя я-а")
            {
                users = users.OrderByDescending(u => u.Email);
            }

            return users;
        }

        private IQueryable<UserViewModel> SelectUsers(
           IQueryable<UserViewModel> users,
           string email,
           string firstName,
           string lastName,
           string libraryName)
        {
            if (email != null)
            {
                users = users.Where(b => b.Email.Contains(email));
            }

            if (firstName != null)
            {
                users = users.Where(b => b.FirstName.Contains(firstName));
            }

            if (lastName != null)
            {
                users = users.Where(b => b.LastName.Contains(lastName));
            }

            if (libraryName != null)
            {
                users = users.Where(b => b.LibraryName.Contains(libraryName));
            }

            return users;
        }
    }
}
