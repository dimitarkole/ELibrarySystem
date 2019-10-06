namespace ELibrarySystem.Services.LibraryAccountServices
{
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using ELibrarySystem.Data.Models;

    public class AddBookService : IAddBookService
    {
        public ApplicationDbContext context;

        public IGenreService genreService;

        public AddBookService(ApplicationDbContext context,
            IGenreService genreService)
        {
            this.context = context;
            this.genreService = genreService;
        }

        public string AddBook(AddBookViewModel model, string userId)
        {
            var genreId = model.GenreId;
            var bookName = model.BookName;
            var author = model.Author;

            var genreObj = this.context.Genres.FirstOrDefault(g =>
                g.Id == genreId
                && g.DeletedOn == null);

            var book = this.context.Books.FirstOrDefault(b =>
                b.BookName == bookName
                && b.Author == author
                && b.UserId == userId
                && b.DeletedOn == null);

            if (book == null)
            {
                var newBook = new Book()
                {
                    BookName = bookName,
                    Author = author,
                    GenreId = genreId,
                    Genre = genreObj,
                    UserId = userId,
                };
                this.context.Books.Add(newBook);
                genreObj.Books.Add(newBook);
                this.context.SaveChanges();
                string result = "Успешно добавена книганата!";
                //AddMessageAtDB(userId, result);
                return result;
            }

            return "Книганата същесвува в библиотеката Ви!";
        }

        public AddBookViewModel PreparedPage()
        {
            var genres = this.genreService.GetAllGenres();

            var genre = new GenreListViewModel()
            {
                Id = null,
                Name = "Изберете жанр",
            };
            genres.Add(genre);
            genres.Reverse();

            var model = new AddBookViewModel()
            {
                Genres = genres,
            };
            return model;
        }

    }
}
