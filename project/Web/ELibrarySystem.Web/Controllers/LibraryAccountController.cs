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
    public class LibraryAccountController : Controller
    {
        private IAddBookService addBookService;
        private IMessageService messageService;
        private IGenreService genreService;
        private IGetAllBooksServices getAllBooks;

        private string userId;

        public string UserId { get => this.userId; set => this.userId = value; }

        public LibraryAccountController(
            IAddBookService addBookService,
            IMessageService messageService,
            IGenreService genreService,
            IGetAllBooksServices getAllBooks)
        {
            this.addBookService = addBookService;
            this.messageService = messageService;
            this.genreService = genreService;
            this.getAllBooks= getAllBooks;
        }

        public void StarUp()
        {
            this.UserId = this.HttpContext.Session.GetString("userId");
            this.ViewBag.UserType = "libary";
            //this.HttpContext.Session.SetString("userId", this.UserId);
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
            this.ViewData["message"] = this.addBookService.AddBook(model, this.userId);
            var returnModel = this.addBookService.PreparedPage();
            return this.View(returnModel);
        }

        //AllBooks Page - view
        [Authorize]
        [HttpGet]
        public IActionResult AllBooks()
        {
            this.StarUp();
            var returnModel = this.getAllBooks.PreparedPage(this.userId);
            return this.View(returnModel);
        }

        //AllBooks Page - search books
        [Authorize]
        [HttpPost]
        public IActionResult AllBooksSearch(AllBooksViewModel model)
        {
            this.StarUp();
            var returnModel = this.getAllBooks.GetBooks(model, this.userId);
            return this.View(returnModel);
        }

    }
}
