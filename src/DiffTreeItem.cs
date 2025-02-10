using DiffEngine;
using System;
using System.Collections.Generic;
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

    internal class DiffTreeItem : TreeViewItem
    {
        public string FullPath { get; set; }

        public DiffDecompileEntry DiffDecompileEntry { get; set; }

        public Brush DefaultBackground { get; set; } = Brushes.White;

        public Brush LastBackground { get; set; }

        public CloudModel Cloud { get; set; }

        public bool IsDiffModuleUnit
        {
            get
            {
                return DiffDecompileEntry == null;
            }
        }

        public IEnumerable<DiffDecompileEntry> Entries { get; set; }
    }
}
