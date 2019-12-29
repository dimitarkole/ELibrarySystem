namespace ELibrarySystem.Web.Controllers
{
    using ELibrarySystem.Services.Messaging;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class SendMailController : Controller
    {

        public SendMailController(ILoggerFactory loggerFactory)
        {
           
        }

        public IActionResult Index()
        {
            
            return this.View();
        }
    }
}
