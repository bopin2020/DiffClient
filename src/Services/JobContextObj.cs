using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DiffClient.Services
{
    internal class JobContextObj : IJobContext
    {
        private MainWindow _mainWindow;
        private TabControl _tabControl;
        private string _message = string.Empty;

        public JobContextObj(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _tabControl = mainWindow.rootTab;
        }

        public MainWindow MainWindow => _mainWindow;

        public TabControl TabControl => _tabControl;

        public string Message { get; set; } = string.Empty;
    }
}
