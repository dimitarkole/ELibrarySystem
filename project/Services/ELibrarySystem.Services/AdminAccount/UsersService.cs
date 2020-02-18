namespace ELibrarySystem.Services.AdminAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.AdminAccount;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.AdminAccount;

    public class UsersService : IUsersService
    {
        private ApplicationDbContext context;

        private IMessageService messageService;

        public UsersService(
            ApplicationDbContext context,
            IMessageService messageService)
        {
            this.context = context;
            this.messageService = messageService;
        }

        public UsersViewModel PreparedPage()
        {
            var model = new UsersViewModel();
            var returnModel = this.GetUsers(model);
            return returnModel;
        }

        public List<object> MakeUserLibrary(UsersViewModel model, string userId)
        {
            var user = this.context.Users
                .FirstOrDefault(u => u.Id == userId && u.DeletedOn == null);
            user.Type = "library";
            this.context.SaveChanges();
            var returnMoodel = this.GetUsers(model);
            List<object> result = new List<object>();
            result.Add(result);
            result.Add("Успешно променени права на потребител!");

            var message = $"Вашите права бяха променени на библиотека";
            this.messageService.AddMessageAtDB(userId, message);

            return result;
        }

        public List<object> MakeLibraryUser(UsersViewModel model, string userId)
        {
            var user = this.context.Users
                .FirstOrDefault(u => u.Id == userId && u.DeletedOn == null);
            user.Type = "user";
            this.context.SaveChanges();
            var returnMoodel = this.GetUsers(model);
            List<object> result = new List<object>();
            result.Add(result);

            var message = $"Вашите права бяха променени на потребител";
            this.messageService.AddMessageAtDB(userId, message);

            result.Add("Успешно променени права на потребител!");
            return result;
        }

        public List<object> MakeUserAdmin(UsersViewModel model, string userId)
        {
            var user = this.context.Users
                .FirstOrDefault(u => u.Id == userId && u.DeletedOn == null);
            user.Type = "admin";
            this.context.SaveChanges();
            var returnMoodel = this.GetUsers(model);
            List<object> result = new List<object>();
            result.Add(result);
            result.Add("Успешно променени права на потребител!");

            var message = $"Вашите права бяха променени на администратор";
            this.messageService.AddMessageAtDB(userId, message);
            return result;
        }

        public List<object> DeleteUser(UsersViewModel model, string userId, string adminId)
        {
            List<object> result = new List<object>();
            var flag = false;
            if (userId != adminId)
            {
                var user = this.context.Users
                    .FirstOrDefault(u => u.Id == userId && u.DeletedOn == null);
                user.DeletedOn = DateTime.UtcNow;
                this.context.SaveChanges();
                flag = true;
            }
          
            var returnMoodel = this.GetUsers(model);
            result.Add(result);
            if (flag == true)
            {
                result.Add("Успешно изтрит потребител!");
                var message = $"Вашият профил беше изтрит успешно";
                this.messageService.AddMessageAtDB(userId, message);
            }
            else
            {
                result.Add("Не може да си изтриите собствения профил!");
            }
            return result;
        }

        public UsersViewModel ChangeActivePage(UsersViewModel model, int newPage)
        {
            model.CurrentPage = newPage;
            return this.GetUsers(model);
        }

        private UsersViewModel GetUsers(UsersViewModel model)
        {
            var email = model.SearchUser.Email;
            var firstName = model.SearchUser.FirstName;
            var lastName = model.SearchUser.LastName;
            var libraryName = model.SearchUser.LibraryName;

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
                    Type = u.Type,
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

            var searchUser = new UserViewModel()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                LibraryName = libraryName,
            };

            var returnModel = new UsersViewModel()
            {
                SearchUser = searchUser,
                CountUsersOfPage = countUsersOfPage,
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
