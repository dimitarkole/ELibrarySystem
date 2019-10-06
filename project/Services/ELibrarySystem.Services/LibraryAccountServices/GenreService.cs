namespace ELibrarySystem.Services.LibraryAccountServices
{
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;
    using System.Collections.Generic;
    using System.Linq;

    public class GenreService : IGenreService
    {
        public ApplicationDbContext context;

        public List<GenreListViewModel> GetAllGenres()
        {
            var genres = this.context.Genres.Select(g => new GenreListViewModel()
            {
                Id = g.Id,
                Name = g.Name,
            }).ToList();
            var result = genres.OrderBy(x => x.Name).ToList();
            return result;
        }
    }
}
