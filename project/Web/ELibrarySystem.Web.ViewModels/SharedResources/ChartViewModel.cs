namespace ELibrarySystem.Web.ViewModels.SharedResources
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ChartViewModel
    {
        public ChartViewModel(List<ChartDataViewModel> chartData)
        {
            this.ChartData = chartData;
        }

        public string StackedDimensionOne { get; set; }

        public List<ChartDataViewModel> ChartData { get; set; }
    }
}
