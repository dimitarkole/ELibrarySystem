namespace ELibrarySystem.Services.LibraryAccountServices
{
    using ELibrarySystem.Data;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MessageService : IMessageService
    {
        public ApplicationDbContext context;

        public MessageService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string AddMessageAtDB(string userId, string textOfMessage)
        {
            var user = this.context.Users.FirstOrDefault(u => u.Id == userId);

            Message message = new Message()
            {
                UserId = userId,
                User = user,
                TextOfMessage = textOfMessage,
            };

            this.context.Messages.Add(message);
            this.context.SaveChanges();
            return message.Id;
        }
    }
}
