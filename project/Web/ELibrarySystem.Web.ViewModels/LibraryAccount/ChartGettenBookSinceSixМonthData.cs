using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrarySystem.Web.ViewModels.LibraryAccount
{
    public class ChartGettenBookSinceSixМonthData
    {
        public ChartGettenBookSinceSixМonthData(string mounth, int gettenBookCount, int returnedBookCount)
        {
            this.Mounth = mounth;
            this.GettenBookCount = gettenBookCount;
            this.ReturnedBookCount = returnedBookCount;
        }

        public string Mounth { get; set; }

        public int GettenBookCount { get; set; }

        public int ReturnedBookCount { get; set; }
    }
}
