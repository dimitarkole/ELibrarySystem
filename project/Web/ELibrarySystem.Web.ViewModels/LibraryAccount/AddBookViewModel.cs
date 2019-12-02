namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class AddBookViewModel
    {
        public string BookId { get; set; }

        [Required(ErrorMessage = "Моля въведете име на книга!")]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Моля въведете име на автора!")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Моля въведете име на автора!")]
        public string CatalogNumber { get; set; }

        [Required(ErrorMessage = "Моля изберете жанр!")]
        public string GenreId { get; set; }

        public string Commentar { get; set; }

        public List<GenreListViewModel> Genres { get; set; }
    }
}
