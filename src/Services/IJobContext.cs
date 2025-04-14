using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DiffClient.Services
{
    internal interface IJobContext
    {
        MainWindow MainWindow { get; }

        TabControl TabControl { get; }
    }
}
