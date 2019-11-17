namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public class IndexLibraryService : IIndexLibraryService
    {
        private ApplicationDbContext context;

        public IndexLibraryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IndexLibraryViewModel PreparedPage(string userId)
        {
            var model = new IndexLibraryViewModel()
            {
                CountAddedBooks = this.CountAddedBooks(userId),
                CountGettedBooks = this.CountAddedBooks(userId),
                CountTakenBooks = this.CountTakenBooks(userId),
            };
            return model;
        }

        private int CountAddedBooks(string userId)
        {
            int count = this.context
                .Books
                .Where(b =>
                    b.UserId == userId
                    && b.DeletedOn == null)
                .Count();
            return count;
        }

        private int CountGettedBooks(string userId)
        {
            int count = this.context
                .GetBooks
                .Where(b =>
                    b.Book.UserId == userId
                    && b.DeletedOn == null)
                .Count();
            return count;
        }

        private int CountTakenBooks(string userId)
        {
            int count = this.context
                .GetBooks
                .Where(b =>
                    b.Book.UserId == userId
                    && b.ReturnedOn == null
                    && b.DeletedOn == null)
                .Count();
            return count;
        }
    }
}
