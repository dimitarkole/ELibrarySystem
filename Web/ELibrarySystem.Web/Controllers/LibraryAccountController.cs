namespace ELibrarySystem.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class LibraryAccountController : Controller
    {
        private ILibraryService libraryService;
        private string userId;

        public string UserId { get => this.userId; set => this.userId = value; }

        // Home Page
        [Authorize]
        public IActionResult Index()
        {
            this.StarUp();
            return this.View();
        }

        public void StarUp()
        {
            this.UserId = this.HttpContext.Session.GetString("userId");
            this.ViewBag.UserType = "libary";
            this.HttpContext.Session.SetString("userId", this.UserId);
        }
    }
}
