namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MessagesViewModel
    {
        public MessagesViewModel()
        {
            this.CountMessagesOfPageList = new List<int>();

            this.CountMessagesOfPageList.Add(10);
            this.CountMessagesOfPageList.Add(20);
            this.CountMessagesOfPageList.Add(30);

            this.CountMessagesOfPage = this.CountMessagesOfPageList[0];
            this.CurrentPage = 1;
        }

        public List<MessageViewModel> Messages { get; set; }

        public int CurrentPage { get; set; }

        public int MaxCountPage { get; set; }

        public int CountMessagesOfPage { get; set; }

        public List<int> CountMessagesOfPageList { get; set; }
    }
}
