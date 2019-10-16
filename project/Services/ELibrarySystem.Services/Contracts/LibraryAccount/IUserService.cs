namespace ELibrarySystem.Services.Contracts.LibraryAccount
{
    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public interface IUserService
    {
        AllUsersViewModel PreparedPage();

        AllUsersViewModel GetUsers(AllUsersViewModel model);

        AllUsersViewModel ChangeActivePage(AllUsersViewModel model, int newPage);

    }
}
