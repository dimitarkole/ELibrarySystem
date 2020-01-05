﻿namespace ELibrarySystem.Services.Contracts.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISendMail
    {
        public bool SendingMail(string toMail, string subject, string messageBody);

        public Dictionary<string, string> VerifyMailTemplate(string url);

        public Dictionary<string, string> ForgotenPasswordSendCode(string url);

        public Dictionary<string, string> NewRegesterUser(string email, string password);

        public void SendMailByTemplate(string toMail, string templateName, Dictionary<string, string> info);
    }
}
