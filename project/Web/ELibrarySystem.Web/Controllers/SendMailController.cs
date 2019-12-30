namespace ELibrarySystem.Web.Controllers
{
    using ELibrarySystem.Services.Contracts.Home;
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
        private readonly ISendMail sendMail;

        public SendMailController(ISendMail sendMail)
        {
            this.sendMail = sendMail;
        }

        public IActionResult Index()
        {
            string toEmail = "dim_kolev2002@abv.bg";
            string subject = "dim_kolev2002@abv.bg";
            string messageBody = "dim_kolev2002@abv.bg";
            var result = this.sendMail.SendingMail(toEmail, subject, messageBody);
            return this.View();
        }
    }
}
