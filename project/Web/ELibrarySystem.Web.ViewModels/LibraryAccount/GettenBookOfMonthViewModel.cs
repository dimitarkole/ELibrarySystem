namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using ELibrarySystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GettenBookOfMonthViewModel
    {
        public GettenBookOfMonthViewModel(GetBook getBook)
        {
            this.Id = getBook.Id;
            this.UserId = getBook.UserId;
            this.User = getBook.User;
            this.BookId = getBook.BookId;
            this.Book = getBook.Book;
            this.Books = getBook.Books;
            this.CreatedOn = getBook.CreatedOn;
            this.ReturnedOn = getBook.ReturnedOn;
            this.DeletedOn = getBook.DeletedOn;

            int month = this.CreatedOn.Month;
            int year = this.CreatedOn.Year;
            this.CreatedOnYearAndMonth = year + " " + month;
            this.CreatedOnMonth = this.MonthToSring(month);
        }

        public string Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string BookId { get; set; }

        public Book Book { get; set; }

        public ICollection<Book> Books { get; set; }

        public DateTime CreatedOn { get; set; }        

        public DateTime? ReturnedOn { get; set; }

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
