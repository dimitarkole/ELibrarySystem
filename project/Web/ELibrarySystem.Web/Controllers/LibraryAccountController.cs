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
        private string userId;

        public string UserId { get => this.userId; set => this.userId = value; }

       /* public LibraryAccountController(
        ILibraryService addBookService)
        {
            this.libraryService = addBookService;
        }*/

        public void StarUp()
        {
            this.UserId = this.HttpContext.Session.GetString("userId");
            this.ViewBag.UserType = "libary";
            this.HttpContext.Session.SetString("userId", this.UserId);
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
        public IActionResult AllBooks()
        {
            this.StarUp();
            var viewModel = new AddBookViewModel()
            {
                //Genres = allGenres,
            };

            return this.View(viewModel);
        }
    }
}
