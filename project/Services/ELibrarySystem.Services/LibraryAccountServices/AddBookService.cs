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

        public AddBookService(ApplicationDbContext context)
        {
            this.context = context;
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
            var genres = this.GetAllGenres().OrderByDescending(x => x.Name).ToList();

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

        public List<GenreListViewModel> GetAllGenres()
        {
            var genres = this.context.Genres.Select(g => new GenreListViewModel()
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();
            var result = genres.OrderBy(x => x.Name).ToList();

            return result;
        }
    }
}
