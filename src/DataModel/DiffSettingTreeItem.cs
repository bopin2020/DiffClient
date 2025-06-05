using DiffEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

#pragma warning disable

namespace DiffClient.DataModel
{
    internal class DiffSettingTreeItem : TreeViewItem
    {
        public string ModuleName { get; set; }

        public Brush DefaultBackground { get; set; } = Brushes.White;

        public Brush LastBackground { get; set; }

        public Page RegisterService { get; set; }
    }
}
