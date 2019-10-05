using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrarySystem.Data.Models
{
    public class Message
    {
        public Message()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public virtual string Id { get; set; }

        public virtual string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual string TextOfMessage { get; set; }

        public virtual DateTime CreatedOn { get; set; }
        public virtual DateTime? SeenOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }
    }
}
