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

    public class LibraryAccountController : Controller
    {
        private IBookService bookService;
        private IMessageService messageService;
        private IGenreService genreService;
        private IAllBooksServices getAllBooks;
        private IGiveBookService giveBookService;
        private SignInManager<ApplicationUser> SignInManager;
        private UserManager<ApplicationUser> UserManager;

        public string UserId;

        public LibraryAccountController(
            IBookService bookService,
            IMessageService messageService,
            IGenreService genreService,
            IAllBooksServices getAllBooks,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IGiveBookService giveBookService)
        {
            this.bookService = bookService;
            this.messageService = messageService;
            this.genreService = genreService;
            this.getAllBooks = getAllBooks;
            this.SignInManager = signInManager;
            this.UserManager = userManager;
            this.giveBookService = giveBookService;
        }

        public void StarUp()
        {
            this.UserId = this.UserManager.GetUserId(this.User);
            this.ViewBag.UserType = "libary";
        }

        // Home Page
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            this.StarUp();
            return this.View();
        }

        // AddBook Page - view
        [Authorize]
        [HttpGet]
        public IActionResult AddBook()
        {
            this.StarUp();
            var returnModel = this.bookService.PreparedAddBookPage();
            return this.View(returnModel);
        }

        // AddBook Page - view
        [Authorize]
        [HttpPost]
        public IActionResult AddBook(AddBookViewModel model)
        {
            this.StarUp();
            this.ViewData["message"] = this.bookService.AddBook(model, this.UserId);
            var returnModel = this.bookService.PreparedAddBookPage();
            return this.View(returnModel);
        }

        // AllBooks Page - view
        [Authorize]
        [HttpGet]
        public IActionResult AllBooks()
        {
            this.StarUp();
            var returnModel = this.getAllBooks.PreparedPage(this.UserId);
            return this.View(returnModel);
        }

        // AllBooks Page - search book
        [Authorize]
        [HttpPost]
        public IActionResult AllBooksSearch(AllBooksViewModel model)
        {
            this.StarUp();
            var returnModel = this.getAllBooks.GetBooks(model, this.UserId);
            return this.View("AllBooks", returnModel);
        }

        // AllBooks Page - Delete book
        [Authorize]
        [HttpPost]
        public IActionResult DeleteBook(AllBooksViewModel model, string id)
        {
            this.StarUp();
            this.ViewData["message"] = "Успешно премахната книга";
            var returnModel = this.getAllBooks.DeleteBook(this.UserId, model, id);

            return this.View("AllBooks", returnModel);
        }

        public IActionResult ChangePageAllBook(AllBooksViewModel model, int id)
        {
            this.StarUp();
            var returnModel = this.getAllBooks.ChangeActivePage(model, this.UserId, id);
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
            var result = this.bookService.EditBook(model, this.UserId);
            var returnModel = result[0];
            this.ViewData["message"] = result[1];
            return this.View(returnModel);
        }

        // Home Page
        [Authorize]
        [HttpGet]
        public IActionResult GiveBook()
        { 
            this.StarUp();
            var model = this.giveBookService.PreparedPage(this.UserId);
            return this.View();
        }
    }
}
