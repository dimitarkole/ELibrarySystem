using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrarySystem.Web.Controllers
{
    public class LibraryAccountController : Controller
    {
        private string userId;

        public LibraryAccountController(
           )
        {
        }

        //Home Page
       // [Authorize]
        public IActionResult Index()
        {

            StarUp();
            return View();
        }

        public void StarUp()
        {
            //userId = HttpContext.Session.GetString("userId");
           // ViewBag.UserType = "libary";
            // HttpContext.Session.SetString("userId",userId);

        }
    }
}
