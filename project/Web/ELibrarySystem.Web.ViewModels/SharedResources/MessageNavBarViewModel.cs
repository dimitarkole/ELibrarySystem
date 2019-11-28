namespace ELibrarySystem.Web.ViewModels.SharedResources
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MessageNavBarViewModel
    {
        public string Id { get; set; }

        public string TextOfMessage { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual DateTime? SeenOn { get; set; }
    }
}
