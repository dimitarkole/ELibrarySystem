using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
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
