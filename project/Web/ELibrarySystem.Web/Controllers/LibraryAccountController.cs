namespace ELibrarySystem.Web.Controllers
{
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using ELibrarySystem.Data.Models;
    using Unity;
    using System.Text;
    using Microsoft.Extensions.Logging;
    using ELibrarySystem.Web.Areas.Identity.Pages.Account;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using ELibrarySystem.Services.Contracts.Home;

    using Microsoft.AspNetCore.Hosting;
    using System.IO;
    using Microsoft.AspNetCore.Http;

    public class LibraryAccountController : Controller
    {
        private readonly IIndexLibraryService indexLibraryService;
        private readonly IBookService bookService;
        private readonly IMessageService messageService;
        private readonly IGenreService genreService;
        private readonly IAllBooksServices getAllBooks;
        private readonly IGiveBookService giveBookService;
        private readonly IGivenBooksService givenBooksService;
        private readonly ILibraryProfileService libraryProfileService;
        private readonly IStatsLibraryService statsLibraryService;
        private readonly IProfileChakerService profilChekerService;

        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LogoutModel> logger;
        private string userId;
        private readonly IHostingEnvironment hostingEnvironment;

        public LibraryAccountController(
            IBookService bookService,
            IMessageService messageService,
            IGenreService genreService,
            IAllBooksServices getAllBooks,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IGiveBookService giveBookService,
            IUserService userService,
            IGivenBooksService givenBooksService,
            ILogger<LogoutModel> logger,
            ILibraryProfileService libraryProfileService,
            IStatsLibraryService statsLibraryService,
            IIndexLibraryService indexLibraryService,
            IHostingEnvironment hostingEnvironment,
            IProfileChakerService profilChekerService)
        {
            this.bookService = bookService;
            this.messageService = messageService;
            this.genreService = genreService;
            this.getAllBooks = getAllBooks;
            this.userManager = userManager;
            this.giveBookService = giveBookService;
            this.userService = userService;
            this.givenBooksService = givenBooksService;
            this.signInManager = signInManager;
            this.logger = logger;
            this.libraryProfileService = libraryProfileService;
            this.statsLibraryService = statsLibraryService;
            this.indexLibraryService = indexLibraryService;
            this.hostingEnvironment = hostingEnvironment;
            this.profilChekerService = profilChekerService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult LogOut()
        {
            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // Home Page
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var model = this.indexLibraryService.PreparedPage(this.userId);
            return this.View(model);
        }

        // AddBook Page - HttpGet
        [Authorize]
        [HttpGet]
        public IActionResult AddBook()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.bookService.PreparedAddBookPage();
            return this.View(returnModel);
        }

        // AddBook Page - HttpPost
        [Authorize]
        [HttpPost]
        public IActionResult AddBook(AddBookViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            this.ViewData["message"] = this.bookService.AddBook(model, this.userId);
            var returnModel = this.bookService.PreparedAddBookPage();
            return this.View(returnModel);
        }

        // AllBooks Page - view
        [Authorize]
        [HttpGet]
        public IActionResult AllBooks()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.getAllBooks.PreparedPage(this.userId);
            return this.View(returnModel);
        }

        // AllBooks Page - search book
        [Authorize]
        [HttpPost]
        public IActionResult AllBooksSearch(AllBooksViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.getAllBooks.GetBooks(model, this.userId);
            return this.View("AllBooks", returnModel);
        }

        // AllBooks Page - Delete book
        [Authorize]
        [HttpPost]
        public IActionResult DeleteBook(AllBooksViewModel model, string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            this.ViewData["message"] = "Успешно премахната книга";
            var returnModel = this.getAllBooks.DeleteBook(this.userId, model, id);

            return this.View("AllBooks", returnModel);
        }

        [Authorize]
        public IActionResult ChangePageAllBook(AllBooksViewModel model, int id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.getAllBooks.ChangeActivePage(model, this.userId, id);
            return this.View("AllBooks", returnModel);
        }

        // AllBooks Page - Edit book
        [Authorize]
        [HttpPost]
        public IActionResult EditBookAllBook(string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var model = this.bookService.GetBookDataById(id);
            this.HttpContext.Session.SetString("editBookId", id);
            return this.View("EditBook", model);
        }

        // AllBooks Page - Edit book
        [Authorize]
        [HttpPost]
        public IActionResult EditBook(AddBookViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var bookId = this.HttpContext.Session.GetString("editBookId");
            model.BookId = bookId;
            var result = this.bookService.EditBook(model, this.userId);
            var returnModel = result[0];
            this.ViewData["message"] = result[1];
            return this.View(returnModel);
        }

        // GiveBook Page
        [Authorize]
        [HttpGet]
        public IActionResult GiveBook()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var model = this.giveBookService.PreparedPage(this.userId);
            return this.View(model);
        }

        // GiveBook Page - GiveBookSearchBook
        [Authorize]
        [HttpPost]
        public IActionResult GiveBookSearchBook(GiveBookViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            var returnModel = this.giveBookService.GiveBookSearchBook(
                model, this.userId, selectedBookId, selecteduserId);
            return this.View("GiveBook", returnModel);
        }

        // GiveBook Page - GiveBookSearchUser
        [Authorize]
        [HttpPost]
        public IActionResult GiveBookSearchUser(GiveBookViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            var returnModel = this.giveBookService.GiveBookSearchUser(
                model, this.userId, selectedBookId, selecteduserId);
            return this.View("GiveBook", returnModel);
        }

        // GiveBook Page - GiveBookChangePageBook
        [Authorize]
        [HttpPost]
        public IActionResult GiveBookChangePageBooks(GiveBookViewModel model, int id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            var returnModel = this.giveBookService.GiveBookChangeBookPage(
                model, this.userId, id, selectedBookId, selecteduserId);
            return this.View("GiveBook", returnModel);
        }

        // GiveBook Page - GiveBookChangePageUser
        [Authorize]
        [HttpPost]
        public IActionResult GiveBookChangePageUsers(GiveBookViewModel model, int id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            var returnModel = this.giveBookService.GiveBookChangeUserPage(
                model, this.userId, id, selectedBookId, selecteduserId);
            return this.View("GiveBook", returnModel);
        }

        // GiveBook Page - GiveBookChangePageUser
        [Authorize]
        [HttpPost]
        public IActionResult SelectBookGiveBookPage(GiveBookViewModel model, string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            var returnModel = this.giveBookService.GiveBookSelectedBook(
                model, this.userId, id, selecteduserId);
            this.HttpContext.Session.SetString("SelectedBookId", returnModel.SelectedBook.BookId);
            return this.View("GiveBook", returnModel);
        }

        // GiveBook Page - SelectUserGiveBookPage
        [Authorize]
        [HttpPost]
        public IActionResult SelectUserGiveBookPage(GiveBookViewModel model, string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");

            var returnModel = this.giveBookService.GiveBookSelectedUser(
                model, this.userId, id, selectedBookId);
            this.HttpContext.Session.SetString("SelecteduserId", returnModel.SelectedUser.UserId);
            return this.View("GiveBook", returnModel);
        }

        // GiveBook Page - GiveBookGivingBook
        [Authorize]
        [HttpPost]
        public IActionResult GiveBookGivingBook(GiveBookViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");

            var returnModel = this.giveBookService.GivingBook(
                model, this.userId, selectedBookId, selecteduserId);
            this.ViewData["message"] = returnModel[0] == null ? "Да" : returnModel[0];
            //return this.View("GiveBook", model);

            return this.View("GiveBook", returnModel[1]);
        }

        // GivenBooks Page - GivenBooks
        [Authorize]
        [HttpGet]
        public IActionResult GivenBooks()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.givenBooksService.PreparedPage(this.userId);

            return this.View(returnModel);
        }

        // GiveBook Page - ChangePageGivenBooks
        [Authorize]
        [HttpPost]
        public IActionResult ChangePageGivenBooks(GivenBooksViewModel model, int id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.givenBooksService.ChangeActivePage(model, this.userId, id);
            return this.View("GivenBooks", returnModel);
        }

        // GiveBook Page - GivenBooksSearch
        [Authorize]
        [HttpPost]
        public IActionResult GivenBooksSearch(GivenBooksViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.givenBooksService.GetGevenBooks(model, this.userId);
            return this.View("GivenBooks", returnModel);
        }

        // GiveBook Page - ReturningGivenBook
        [Authorize]
        [HttpPost]
        public IActionResult ReturningGivenBook(GivenBooksViewModel model, string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.givenBooksService.ReturningBook(model, this.userId, id);
            this.ViewData["message"] = returnModel[1];
            return this.View("GivenBooks", returnModel[0]);
        }

        // GiveBook Page - DeleteGivenBook
        [Authorize]
        [HttpPost]
        public IActionResult DeleteGivenBook(GivenBooksViewModel model, string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.givenBooksService.DeletingBook(model, this.userId, id);
            return this.View("GivenBooks", returnModel[0]);
        }


        // GiveBook Page - SendMessageForReturningBook
        [Authorize]
        [HttpPost]
        public IActionResult SendMessageForReturningBook(GivenBooksViewModel model, string id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.givenBooksService.SendMessageForReturningBook(model, this.userId, id);
            this.ViewData["message"] = returnModel[1];
            return this.View("GivenBooks", returnModel[0]);
        }

        // Profile
        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.libraryProfileService.PreparedPage(this.userId);
            return this.View(returnModel);
        }

        // Profile
        [Authorize]
        [HttpPost]
        public IActionResult Profile(ProfilLibraryViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var pic = model.Photo;
            if (pic != null)
            {
                var fileName = Path.Combine(
                    this.hostingEnvironment.WebRootPath + "/img/Avatars",
                    Path.GetFileName(this.userId + "_" + pic.FileName));
                pic.CopyTo(new FileStream(fileName, FileMode.Create));
                model.AvatarLocation = "/img/Avatars/" + Path.GetFileName(fileName);
            }

            var returnModel = this.libraryProfileService.SaveChanges(model, this.userId);

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ProfilLibraryViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var changPassword = model.ResetPasswordViewModel;

            var changePasswordResult = await this.userManager.ChangePasswordAsync(user, changPassword.OldPassword, changPassword.NewPassword);
            var returnModel = this.libraryProfileService.PreparedPage(this.userId);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                this.ViewData["message"] = "Неуспешно сменяне на парола!";
                return this.View("Profile", returnModel);
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.ViewData["message"] = "Успешно сменена на парола!";

            return this.View("Profile", returnModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Stats()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.statsLibraryService.PreparedPage(this.userId);
            this.ViewData["message"] = this.userId;
            return this.View(returnModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult StatsSearch(StatsLibaryViewModel model)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.statsLibraryService.SearchStats(model, this.userId);
            this.ViewData["message"] = this.userId;
            return this.View("Stats", returnModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Notification()
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.messageService.GetMessagesPreparedPage(this.userId);
            return this.View(returnModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult NotificationChangePage(MessagesViewModel model, int id)
        {
            var startUp = this.StartUp();
            if (startUp != null)
            {
                return startUp;
            }

            var returnModel = this.messageService.GetMessagesChangePage(model, this.userId, id);
            this.StartUp();

            return this.View("Notification", returnModel);
        }

        private IActionResult StartUp()
        {
            this.userId = this.userManager.GetUserId(this.User);
            this.ViewData["UserType"] = "library";
            var chackProfile = this.profilChekerService.CheckCurrectAccount(this.userId, "library");
            if (chackProfile != null)
            {
                if (chackProfile == "admin")
                {
                    return this.RedirectToAction(nameof(AdminAccountController.Index), "AdminAccount");
                }
                else if (chackProfile == "library")
                {
                    return this.RedirectToAction(nameof(LibraryAccountController.Index), "LibraryAccount");
                }
                else if (chackProfile == "user")
                {
                    return this.RedirectToAction(nameof(UserAccountController.Index), "UserAccount");

                }
                else
                {
                    return this.LogOut();
                }
            }

            var messages = this.messageService.GetMessagesNavBar(this.userId);
            this.ViewData["MessageNavBar"] = messages;
            return null;
        }
    }
}
