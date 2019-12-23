namespace ELibrarySystem.Services.LibraryAccountServices
{
    using ELibrarySystem.Data;
    using ELibrarySystem.Data.Models;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.SharedResources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MessageService : IMessageService
    {
        private ApplicationDbContext context;

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
                SeenOn = null,
            };

            this.context.Messages.Add(message);
            this.context.SaveChanges();
            return message.Id;
        }

        public MessagesNavBarViewModel GetMessagesNavBar(string userId)
        {
            var messages = this.context.Messages
                .Where(m =>
                    m.DeletedOn == null
                    && m.UserId == userId
                    && m.SeenOn == null)
                .Select(m => new MessageNavBarViewModel()
                    {
                         CreatedOn = m.CreatedOn,
                         Id = m.Id,
                         TextOfMessage = m.TextOfMessage,
                         SeenOn = m.SeenOn,
                    })
                .ToList();

            var result = new MessagesNavBarViewModel(messages);

            return result;
        }

        public MessagesViewModel GetMessagesChangePage(MessagesViewModel model, string userId, int pageIndex)
        {
            var messages = this.context.Messages
               .Where(m =>
                   m.DeletedOn == null
                   && m.UserId == userId)
               .OrderBy(m => m.SeenOn)
               .ThenByDescending(m => m.CreatedOn)
               .Select(m => new MessageViewModel()
               {
                   Id = m.Id,
                   CreatedOn = m.CreatedOn,
                   TextOfMessage = m.TextOfMessage,
                   SeenOn = m.SeenOn,
               })
               .ToList();
            var seenChacker = messages.FirstOrDefault(m => m.SeenOn == null);
            if (seenChacker != null)
            {
                messages = messages.OrderBy(m => m.SeenOn)
                   .ThenByDescending(m => m.CreatedOn).ToList();
            }
            else
            {
                messages = messages.OrderByDescending(m => m.CreatedOn).ToList();
            }

            int countBooksOfPage = model.CountMessagesOfPage;
            int currentPage = pageIndex;

            int maxCountPage = messages.Count() / countBooksOfPage;
            if (messages.Count() % countBooksOfPage != 0)
            {
                maxCountPage++;
            }

            var viewMessages = messages.Skip((currentPage - 1) * countBooksOfPage)
                                .Take(countBooksOfPage).ToList();

            foreach (var message in viewMessages)
            {
                var messageContext = this.context.Messages.FirstOrDefault(m => m.Id == message.Id);
                messageContext.SeenOn = DateTime.UtcNow;
                this.context.SaveChanges();
            }

            var result = new MessagesViewModel()
            {
                Messages = viewMessages,
                CountMessagesOfPage = countBooksOfPage,
                MaxCountPage = maxCountPage,
            };

            return result;
        }

        public MessagesViewModel GetMessagesPreparedPage(string userId)
        {
            var model = new MessagesViewModel();
            var returnModel = this.GetMessagesChangePage(model, userId, 1);
            return returnModel;
        }
    }
}
