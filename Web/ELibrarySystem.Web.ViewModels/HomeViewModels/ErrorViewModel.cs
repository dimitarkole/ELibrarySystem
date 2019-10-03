using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrarySystem.Web.ViewModels.HomeViewModels
{
    class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
