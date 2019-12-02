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

    public class LibraryAccountController : Controller
    {
        private IIndexLibraryService indexLibraryService;
        private IBookService bookService;
        private IMessageService messageService;
        private IGenreService genreService;
        private IAllBooksServices getAllBooks;
        private IGiveBookService giveBookService;
        private IGivenBooksService givenBooksService;
        private ILibraryProfileService libraryProfileService;
        private IStatsLibraryService statsLibraryService;

        private IUserService userService;
        private UserManager<ApplicationUser> userManager;
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
            IHostingEnvironment hostingEnvironment)
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
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await this.signInManager.SignOutAsync();
            this.logger.LogInformation("User logged out.");

            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // Home Page
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            this.StarUp();
            var model = this.indexLibraryService.PreparedPage(this.userId);
            return this.View(model);
        }

        // AddBook Page - HttpGet
        [Authorize]
        [HttpGet]
        public IActionResult AddBook()
        {
            this.StarUp();
            var returnModel = this.bookService.PreparedAddBookPage();
            return this.View(returnModel);
        }

        // AddBook Page - HttpPost
        [Authorize]
        [HttpPost]
        public IActionResult AddBook(AddBookViewModel model)
        {
            this.StarUp();
            this.ViewData["message"] = this.bookService.AddBook(model, this.userId);
            var returnModel = this.bookService.PreparedAddBookPage();
            return this.View(returnModel);
        }

        // AllBooks Page - view
        [Authorize]
        [HttpGet]
        public IActionResult AllBooks()
        {
            this.StarUp();
            var returnModel = this.getAllBooks.PreparedPage(this.userId);
            return this.View(returnModel);
        }

        // AllBooks Page - search book
        [Authorize]
        [HttpPost]
        public IActionResult AllBooksSearch(AllBooksViewModel model)
        {
            this.StarUp();
            var returnModel = this.getAllBooks.GetBooks(model, this.userId);
            return this.View("AllBooks", returnModel);
        }

        // AllBooks Page - Delete book
        [Authorize]
        [HttpPost]
        public IActionResult DeleteBook(AllBooksViewModel model, string id)
        {
            this.StarUp();
            this.ViewData["message"] = "Успешно премахната книга";
            var returnModel = this.getAllBooks.DeleteBook(this.userId, model, id);

            return this.View("AllBooks", returnModel);
        }

        [Authorize]
        public IActionResult ChangePageAllBook(AllBooksViewModel model, int id)
        {
            this.StarUp();
            var returnModel = this.getAllBooks.ChangeActivePage(model, this.userId, id);
            return this.View("AllBooks", returnModel);
        }

        // AllBooks Page - Edit book
        [Authorize]
        [HttpPost]
        public IActionResult EditBookAllBook(string id)
        {
            this.StarUp();
            var model = this.bookService.GetBookDataById(id);
            this.HttpContext.Session.SetString("editBookId", id);
            return this.View("EditBook", model);
        }

        // AllBooks Page - Edit book
        [Authorize]
        [HttpPost]
        public IActionResult EditBook(AddBookViewModel model)
        {
            this.StarUp();
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
            this.StarUp();
            var model = this.giveBookService.PreparedPage(this.userId);
            return this.View(model);
        }

        // GiveBook Page - GiveBookSearchBook
        [Authorize]
        [HttpPost]
        public IActionResult GiveBookSearchBook(GiveBookViewModel model)
        {
            this.StarUp();
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
            this.StarUp();
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
            this.StarUp();
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
            this.StarUp();
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
            this.StarUp();
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
            this.StarUp();
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
            this.StarUp();
            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");

            var returnModel = this.giveBookService.GivingBook(
                model, this.userId, selectedBookId, selecteduserId);
            this.ViewData["message"] = returnModel[0];
            return this.View("GiveBook", returnModel[1]);
        }

        // GivenBooks Page - GiveBookSearchBook
        [Authorize]
        [HttpGet]
        public IActionResult GivenBooks()
        {
            this.StarUp();
            var returnModel = this.givenBooksService.PreparedPage(this.userId);

            return this.View(returnModel);
        }

          // GiveBook Page - GiveBookGivingBook
        [Authorize]
        [HttpPost]
        public IActionResult ChangePageGivenBooks(GivenBooksViewModel model, int id)
        {
            this.StarUp();
            var returnModel = this.givenBooksService.ChangeActivePage(model, this.userId, id);
            return this.View("GivenBooks", returnModel);
        }

        // GiveBook Page - GiveBookGivingBook
        [Authorize]
        [HttpPost]
        public IActionResult GivenBooksSearch(GivenBooksViewModel model)
        {
            this.StarUp();
            var returnModel = this.givenBooksService.GetGevenBooks(model, this.userId);
            return this.View("GivenBooks", returnModel);
        }

        // GiveBook Page - GiveBookGivingBook
        [Authorize]
        [HttpPost]
        public IActionResult ReturnigGivenBook(GivenBooksViewModel model, string id)
        {
            this.StarUp();
            var returnModel = this.givenBooksService.ReturningBook(model, this.userId, id);
            return this.View("GivenBooks", returnModel[0]);
        }

        // GiveBook Page - GiveBookGivingBook
        [Authorize]
        [HttpPost]
        public IActionResult DeleteGivenBook(GivenBooksViewModel model, string id)
        {
            this.StarUp();
            var returnModel = this.givenBooksService.DeletingBook(model, this.userId, id);
            return this.View("GivenBooks", returnModel[0]);
        }

        // Edit Given Book Page

        // GiveBook Page
        [Authorize]
        [HttpGet]
        public IActionResult EditGiveBook()
        {
            this.StarUp();
            var model = this.giveBookService.PreparedPage(this.userId);
            return this.View(model);
        }

        // GiveBook Page - GiveBookSearchBook
        [Authorize]
        [HttpPost]
        public IActionResult EditGiveBookSearchBook(GiveBookViewModel model)
        {
            this.StarUp();
            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            var returnModel = this.giveBookService.GiveBookSearchBook(
                model, this.userId, selectedBookId, selecteduserId);

            // this.ViewData["message"] = sb.ToString().Trim();
            return this.View("EditGiveBook", returnModel);
        }

        // GiveBook Page - GiveBookSearchUser
        [Authorize]
        [HttpPost]
        public IActionResult EditGiveBookSearchUser(GiveBookViewModel model)
        {
            this.StarUp();
            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            var returnModel = this.giveBookService.GiveBookSearchUser(
                model, this.userId, selectedBookId, selecteduserId);
            return this.View("EditGiveBook", returnModel);
        }

        // GiveBook Page - GiveBookChangePageBook
        [Authorize]
        [HttpPost]
        public IActionResult EditGiveBookChangePageBook(GiveBookViewModel model, int id)
        {
            this.StarUp();
            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            var returnModel = this.giveBookService.GiveBookChangeBookPage(
                model, this.userId, id, selectedBookId, selecteduserId);
            return this.View("EditGiveBook", returnModel);
        }

        // GiveBook Page - GiveBookChangePageUser
        [Authorize]
        [HttpPost]
        public IActionResult EditGiveBookChangePageUser(GiveBookViewModel model, int id)
        {
            this.StarUp();
            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            var returnModel = this.giveBookService.GiveBookChangeUserPage(
                model, this.userId, id, selectedBookId, selecteduserId);
            return this.View("EditGiveBook", returnModel);
        }

        // GiveBook Page - GiveBookChangePageUser
        [Authorize]
        [HttpPost]
        public IActionResult EditSelectBookGiveBookPage(GiveBookViewModel model, string id)
        {
            this.StarUp();
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            var returnModel = this.giveBookService.GiveBookSelectedBook(
                model, this.userId, id, selecteduserId);
            this.HttpContext.Session.SetString("SelectedBookId", returnModel.SelectedBook.BookId);
            return this.View("EditGiveBook", returnModel);
        }

        // GiveBook Page - SelectUserGiveBookPage
        [Authorize]
        [HttpPost]
        public IActionResult EditSelectUserGiveBookPage(GiveBookViewModel model, string id)
        {
            this.StarUp();
            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");

            var returnModel = this.giveBookService.GiveBookSelectedUser(
                model, this.userId, id, selectedBookId);
            this.HttpContext.Session.SetString("SelecteduserId", returnModel.SelectedUser.UserId);
            return this.View("EditGiveBook", returnModel);
        }

        // GiveBook Page - GiveBookGivingBook
        [Authorize]
        [HttpPost]
        public IActionResult EditGiveBookEditingBook(GiveBookViewModel model)
        {
            this.StarUp();
            string selectedBookId = this.HttpContext.Session.GetString("SelectedBookId");
            string selecteduserId = this.HttpContext.Session.GetString("SelecteduserId");
            string givenBookId = this.HttpContext.Session.GetString("givenBookId");
            var returnModel = this.giveBookService.EditingGivinBook(
                model, this.userId, givenBookId, selectedBookId, selecteduserId);
            this.ViewData["message"] = returnModel[0];
            return this.View("EditGiveBook", returnModel[1]);
        }

        // Profile
        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            this.StarUp();
            var returnModel = this.libraryProfileService.PreparedPage(this.userId);
            return this.View(returnModel);
        }

        // Profile
        [Authorize]
        [HttpPost]
        public IActionResult Profile(ProfilLibraryViewModel model)
        {
            this.StarUp();
            var returnModel = this.libraryProfileService.SaveChanges(model, this.userId);
            string uploadFile = Path.Combine(this.hostingEnvironment.WebRootPath, "/image");
            this.ViewData["message"] = returnModel[0];
            return this.View(returnModel[0]);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Stats()
        {
            this.StarUp();
            var returnModel = this.statsLibraryService.PreparedPage(this.userId);
            this.ViewData["message"] = this.userId;
            return this.View(returnModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult StatsSearch(StatsLibaryViewModel model)
        {
            this.StarUp();
            var returnModel = this.statsLibraryService.SearchStats(model, this.userId);
            this.ViewData["message"] = this.userId;
            return this.View("Stats", returnModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Messages()
        {
            this.StarUp();
           // var returnModel = this.messageService.GetMessagesPreparedPage(this.userId);
            //return this.View("Messages",  returnModel);
            return this.View();
        }

        private void StarUp()
        {
            this.userId = this.userManager.GetUserId(this.User);
            this.ViewBag.UserType = "libary";

            var messages = this.messageService.GetMessagesNavBar(this.userId);
            this.ViewData["MessageNavBar"] = messages;
        }
    }
}
