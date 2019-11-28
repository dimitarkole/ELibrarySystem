namespace ELibrarySystem.Web.ViewModels.SharedResources
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MessagesNavBarViewModel
    {
        public MessagesNavBarViewModel(List<MessageNavBarViewModel> messages)
        {
            this.Messages = messages;
            this.CountMessages = messages.Count;
        }

        public int CountMessages { get; set; }

        public List<MessageNavBarViewModel> Messages { get; set; }
    }
}
