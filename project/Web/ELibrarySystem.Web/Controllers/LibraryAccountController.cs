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
        private IAddBookService addBookService;
        private IMessageService messageService;
        private IGenreService genreService;
        private IGetAllBooksServices getAllBooks;
        private SignInManager<ApplicationUser> SignInManager;
        private UserManager<ApplicationUser> UserManager;

        public string UserId;

        public LibraryAccountController(
            IAddBookService addBookService,
            IMessageService messageService,
            IGenreService genreService,
            IGetAllBooksServices getAllBooks,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.addBookService = addBookService;
            this.messageService = messageService;
            this.genreService = genreService;
            this.getAllBooks = getAllBooks;
            this.SignInManager = signInManager;
            this.UserManager = userManager;

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

        //AddBook Page - view

        [Authorize]
        [HttpGet]
        public IActionResult AddBook()
        {
            this.StarUp();
            var returnModel = this.addBookService.PreparedPage();
            return this.View(returnModel);
        }

        //AddBook Page - view

        [Authorize]
        [HttpPost]
        public IActionResult AddBook(AddBookViewModel model)
        {
            this.StarUp();
            this.ViewData["message"] = this.addBookService.AddBook(model, this.UserId);
            var returnModel = this.addBookService.PreparedPage();
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

    }
}
