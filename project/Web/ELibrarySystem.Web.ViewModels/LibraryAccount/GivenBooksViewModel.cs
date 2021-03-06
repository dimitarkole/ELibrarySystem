﻿namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GivenBooksViewModel
    {
        public GivenBooksViewModel()
        {
            this.SortMethods = new List<string>();
            this.SortMethods.Add("Име на книгата а-я");
            this.SortMethods.Add("Име на книгата я-а");
            this.SortMethods.Add("Име на автора а-я");
            this.SortMethods.Add("Име на автора я-а");
            this.SortMethods.Add("Жанр а-я");
            this.SortMethods.Add("Жанр я-а");
            this.SortMethodId = this.SortMethods[0];

            this.CountBooksOfPageList = new List<int>();
            this.CountBooksOfPageList.Add(10);
            this.CountBooksOfPageList.Add(15);
            this.CountBooksOfPageList.Add(20);

            this.CountBooksOfPage = this.CountBooksOfPageList[0];
            this.SortMethodId = this.SortMethods[0];
            this.CurrentPage = 1;

            this.SearchGivenBook = new GivenBookViewModel();
        }

        public GivenBookViewModel SearchGivenBook { get; set; }

        public string SortMethodId { get; set; }

        public List<string> SortMethods { get; set; }

        public List<GenreListViewModel> Genres { get; set; }

        public IEnumerable<GivenBookViewModel> Books { get; set; }

        public int CurrentPage { get; set; }

        public int MaxCountPage { get; set; }

        public int CountBooksOfPage { get; set; }

        public List<int> CountBooksOfPageList { get; set; }
    }
}
