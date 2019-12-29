namespace ELibrarySystem.Services.Contracts.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISendMail
    {
        public bool SendingMail(string toMail, string subject, string messageBody);
    }
}
