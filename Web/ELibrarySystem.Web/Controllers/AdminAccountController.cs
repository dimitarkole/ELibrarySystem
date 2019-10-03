using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrarySystem.Web.Controllers
{
    public class AdminAccountController:Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl=null)
        {
            return View();
        }
    }
}
