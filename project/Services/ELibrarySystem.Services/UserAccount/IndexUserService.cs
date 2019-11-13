namespace ELibrarySystem.Services.UserAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ELibrarySystem.Data;
    using ELibrarySystem.Services.Contracts.UserAccount;
    using ELibrarySystem.Web.ViewModels.UserAccount;

    public class IndexUserService : IIndexUserService
    {
        public ApplicationDbContext context;


        public IndexUserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IndexUserViewModel PreparedPage(string userId)
        {
            IndexUserViewModel indexUserViewModel = new IndexUserViewModel()
            {
                CountReadedBooks = this.CountReadedBooks(userId),
                CountTakenBooks = this.CountTakenBooks(userId),
                CountReadingBooks = this.CountReadingBooks(userId),
            };

            throw new NotImplementedException();
        }

        private int CountReadedBooks(string userId)
        {
            int count = this.context
                .GetBooks
                .Where(gb =>
                    gb.UserId == userId
                    && gb.DeletedOn == null)
                .Count();
            return count;
        }

        private int CountTakenBooks(string userId)
        {
            int count = this.context
                .GetBooks
                .Where(gb =>
                    gb.UserId == userId
                    && gb.ReturnedOn == null
                    && gb.DeletedOn == null)
                .Count();
            return count;
        }

        private int CountReadingBooks(string userId)
        {
            var countReadedBooks = this.CountReadedBooks(userId);
            var countTakenBooks = this.CountTakenBooks(userId);
            int count = countReadedBooks - countTakenBooks;
            return count;
        }
    }
}
