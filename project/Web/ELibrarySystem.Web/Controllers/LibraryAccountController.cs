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

    public class LibraryAccountController : Controller
    {
        private IBookService bookService;
        private IMessageService messageService;
        private IGenreService genreService;
        private IAllBooksServices getAllBooks;
        private IGiveBookService giveBookService;
        private IUserService userService;
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
            IGiveBookService giveBookService,
            IUserService userService)
        {
            this.bookService = bookService;
            this.messageService = messageService;
            this.genreService = genreService;
            this.getAllBooks = getAllBooks;
            this.SignInManager = signInManager;
            this.UserManager = userManager;
            this.giveBookService = giveBookService;
            this.userService = userService;
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

        [Authorize]
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

        // GiveBook Page
        [Authorize]
        [HttpGet]
        public IActionResult GiveBook()
        {
            this.StarUp();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"HttpGet GiveBook");
            this.ViewData["message"] = sb.ToString().Trim();
            var model = this.giveBookService.PreparedPage(this.UserId);
            return this.View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GiveBook2()
        {
            this.StarUp();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"HttpGet GiveBook");
            this.ViewData["message"] = sb.ToString().Trim();
            var model = this.giveBookService.PreparedPage(this.UserId);
            return this.View(model);
        }

        // GiveBook Page - GiveBook
        [Authorize]
        [HttpPost]
        public IActionResult GiveBook2(AllBooksViewModel bookModel)
        {
            this.StarUp();
            var returnBookModel = this.getAllBooks.GetBooks(bookModel, this.UserId);
            var returnModel = new GiveBookViewModel()
            {
                AllBooks = returnBookModel,
               //BookPage AllUsers = new AllUsersViewModel(),
            };
            // this.giveBookService.GiveBookSearchBook(bookModel, this.UserId);
            return this.View(returnModel);
        }

        // GiveBook Page - GiveBookSearchBook
        [Authorize]
        [HttpPost]
        public IActionResult GiveBookSearchBook(AllUsersViewModel usersModel,AllBooksViewModel booksModel)
        {
            this.StarUp();
            var model = new GiveBookViewModel()
            {
                AllUsers = usersModel,
                AllBooks = booksModel,
            };

            StringBuilder sb = new StringBuilder();
            string a = model.AllBooks.BookName == null ? "NULl" : "Notnull";
            sb.AppendLine($"HttpPost GiveBookSearchBook {a}");

            sb.AppendLine($"bookName={model.AllBooks.BookName} ");
            sb.AppendLine($"Author={model.AllBooks.Author} ");
            sb.AppendLine($"GenreId={model.AllBooks.GenreId} ");
            this.ViewData["message"] = sb.ToString().Trim();
            var returnModel = this.giveBookService.GiveBookSearchBook(model, this.UserId);
            return this.View("GiveBook", returnModel);
        }
    }
}
