using DiffClient.DataModel;
using DiffClient.UserControls;
using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DiffClient.Commands
{
    internal class RemoveDiffTabItemCommand : EnhancedTabItemCommand
    {
        public RemoveDiffTabItemCommand(MainWindow mainWindow) : base(mainWindow) { }
    }
}
