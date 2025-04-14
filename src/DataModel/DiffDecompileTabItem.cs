using DiffClient.Commands;
using DiffClient.UserControls;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#pragma warning disable

namespace DiffClient.DataModel
{
    internal class DiffDecompileTabItem : EnhancedTabItem<DiffDecompile, DiffDecompileViewModel>
    {
        public DiffDecompileTabItem(MainWindow mainWindow, DiffDecompileArgs args) : base(mainWindow, args?.Title?.ToString(), args) { }
    }
}
