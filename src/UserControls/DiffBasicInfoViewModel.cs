using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.UserControls
{
    internal class DiffBasicInfoViewModel : BaseUserControlModel, IViewModel
    {
        private DiffBasicInfoView _view;

        public string? Header { get; set; }

        public string[] ArchSources { get; set; } = new string[]
        {
            "x86",
            "x86_64",
            "ARM32",
            "ARM64",
            "Unknown"
        };

        public string[] FileSizeType { get; set; } = new string[]
        {
            "b",
            "kb",
            "mb",
            "gb"
        };

        public ulong Size { get; set; }

        public DiffBasicInfoViewModel(DiffBasicInfoView diffBasicInfoView)
        {
            _view = diffBasicInfoView;
        }
    }
}
