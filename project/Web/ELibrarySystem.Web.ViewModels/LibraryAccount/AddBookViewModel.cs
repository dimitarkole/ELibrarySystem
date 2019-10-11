namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class AddBookViewModel
    {
        public AddBookViewModel()
        {
            this.BookId = " ";
        }
        
        public string BookId { get; set; }

        [Required]
        public string BookName { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string GenreId { get; set; }

        public List<GenreListViewModel> Genres { get; set; }
    }
}
