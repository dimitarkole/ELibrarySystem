namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ELibrarySystem.Data.Models;

    public class AddedBookOfMonthViewModel
    {
        public AddedBookOfMonthViewModel(Book book)
        {
            this.BookName = book.BookName;
            this.Author = book.Author;
            this.GenreId = book.GenreId;
            this.UserId = book.UserId;
            this.Genre = book.Genre;
            this.CreatedOn = book.CreatedOn;
            this.DeletedOn = book.DeletedOn;

            int month = this.CreatedOn.Month;
            int year = this.CreatedOn.Year;
            this.CreatedOnYearAndMonth = year + " " + month;
            this.CreatedOnMonth = this.MonthToSring(month);
        }

        public string BookName { get; set; }

        public string Author { get; set; }

        public string GenreId { get; set; }

        public string UserId { get; set; }

        public Genre Genre { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string CreatedOnYearAndMonth { get; set; }

        public string CreatedOnMonth { get; set; }

        private string MonthToSring(int month)
        {
            string result;
            switch (month)
            {
                case 1: result = "Януари"; break;
                case 2: result = "Февруари"; break;
                case 3: result = "Март"; break;
                case 4: result = "Април"; break;
                case 5: result = "Май"; break;
                case 6: result = "Юни"; break;
                case 7: result = "Юли"; break;
                case 8: result = "Август"; break;
                case 9: result = "Септември"; break;
                case 10: result = "Октомври"; break;
                case 11: result = "Ноември"; break;
                case 12: result = "Декември"; break;
                default:
                    result = "null";
                    break;
            }

            return result;
        }
    }
}
