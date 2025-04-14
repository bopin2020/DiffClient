using DiffEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

#pragma warning disable 8618

namespace DiffClient
{
    internal class CloudModel
    {
        public bool IsCloud { get; set; }

        public bool Initialized { get; set; }
    }
    internal class GroupDiffTreeItem : TreeViewItem
    {
        public ObservableCollection<GroupDiffTreeItem> Children { get; set; } = new ObservableCollection<GroupDiffTreeItem>();
    }

    internal class DiffTreeItem : TreeViewItem
    {
        public string FullPath { get; set; }

        public DiffDecompileEntry DiffDecompileEntry { get; set; }

        public Brush DefaultBackground { get; set; } = Brushes.White;

        public Brush LastBackground { get; set; }

        public CloudModel Cloud { get; set; } = new CloudModel();

        public bool IsDiffModuleUnit
        {
            get
            {
                return DiffDecompileEntry == null;
            }
        }

        public IEnumerable<DiffDecompileEntry> Entries { get; set; }

        public string OS { get; set; }

        public string Date { get; set; }

        public bool IsLocal { get; set; }

        public TreeItemType Type { get; set; }

        public ObservableCollection<DiffTreeItem> Children { get; private set; } = new ObservableCollection<DiffTreeItem>();

        public bool IsShowParent { get; set; } = false;

        public bool ContainsEntry { get; set; }

        public bool IsLocalInit { get; set; } = false;

        public void AddProxy(DiffTreeItem di)
        {
            this.Items.Add(di);
            this.Children.Add(di);
        }
    }

    internal enum TreeItemType : int
    {
        None = 0x0000,
        Local = 0x0001,
        Cloud = 0x0002,
        Group = 0x0004,
        TreeView = 0x0008,
        Function = 0x0010,
        LocalDirectory = 0x0020,
        LocalRoot = 0x0040,
        LocalTreeItem = 0x0080,
    }
}
