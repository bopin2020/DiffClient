using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Windows
{
    internal class ExitDialogWindowViewModel
    {
        ExitDialogWindow _exitDialogWindow;
        public ExitDialogWindowViewModel(ExitDialogWindow exitDialogWindow)
        {
            _exitDialogWindow = exitDialogWindow;
        }

        public string ExitDialogDescription { get; set; } = "DiffClient will save all changes to the disk";

        public string NotPackDesc { get; set; } = "Not pack,store the current session to local disk";
        public string PackDesc1 { get; set; } = "Pack the current session to local disk (crypt store)";
        public string PackDesc2 { get; set; } = "Pack the current session to local disk";
        public string NotMemory { get; set; } = "The next will not give you a dialog";
        public string NotStoreDB { get; set; } = "Not store the current session to database";
    }
}
