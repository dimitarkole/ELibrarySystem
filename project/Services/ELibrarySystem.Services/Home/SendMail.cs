namespace ELibrarySystem.Services.Home
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Text;

    using ELibrarySystem.Services.Contracts.Home;

    public class SendMail : ISendMail
    {
        private readonly string fromEmail = "elibrarysite2019@gmail.com";
        private readonly string password = "Elibrary2019!";

        public bool SendingMail(string toMail, string subject, string messageBody)
        {
            try
            {
                // Create smtp connection.
                SmtpClient client = new SmtpClient();

                // outgoing port for the mail.
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(this.fromEmail, this.password);

                // Fill the mail form.
                var send_mail = new MailMessage();
                send_mail.IsBodyHtml = true;

                // address from where mail will be sent.
                send_mail.From = new MailAddress(this.fromEmail);

                // address to which mail will be sent.
                send_mail.To.Add(new MailAddress(toMail));

                // subject and body of the mail.
                send_mail.Subject = subject;
                send_mail.Body = messageBody;

                client.Send(send_mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
