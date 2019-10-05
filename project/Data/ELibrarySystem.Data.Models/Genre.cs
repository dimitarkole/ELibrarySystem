using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrarySystem.Data.Models
{
    public class Genre
    {


        public Genre()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.Books = new List<Book>();
        }
        //
        // Summary:
        //     Gets or sets the date and time, in UTC, when any user lockout ends.
        //
        // Remarks:
        //     A value in the past means the user is not locked out.
        public virtual string Id { get; set; }

        //
        // Summary:
        //     Gets or sets a flag indicating if the user could be locked out.
        public virtual string Name { get; set; }

        public virtual DateTime CreatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
