using DiffClient.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DiffClient.Advanced.RememberRestore
{
    internal interface IRemember
    {
        DiffStatus DumpTreeView(TreeView treeView);

        DiffStatus DumpTabControl(TabControl tabControl);
    }
}
