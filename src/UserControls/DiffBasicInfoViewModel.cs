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

        public string Header { get; set; }

        public DiffBasicInfoViewModel(DiffBasicInfoView diffBasicInfoView)
        {
            _view = diffBasicInfoView;
        }
    }
}
