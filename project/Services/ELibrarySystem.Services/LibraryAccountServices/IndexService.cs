namespace ELibrarySystem.Services.LibraryAccountServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.LibraryAccount;
    using ELibrarySystem.Web.ViewModels.LibraryAccount;

    public class IndexService : IIndexService
    {
        public ApplicationDbContext context;


        public IndexService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IndexViewModel PreparedPage(string userId)
        {
            var model = new IndexViewModel()
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
                .Where(b => b.UserId == userId)
                .Count();
            return count;
        }

        private int CountGettedBooks(string userId)
        {
            int count = this.context
                .GetBooks
                .Where(b => b.Book.UserId == userId)
                .Count();
            return count;
        }

        private int CountTakenBooks(string userId)
        {
            int count = this.context
                .GetBooks
                .Where(b => b.Book.UserId == userId && b.ReturnedOn == null)
                .Count();
            return count;
        }
    }
}
