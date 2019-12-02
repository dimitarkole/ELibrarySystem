namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MessageViewModel
    {
        public string TextOfMessage { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual DateTime? SeenOn { get; set; }
    }
}
