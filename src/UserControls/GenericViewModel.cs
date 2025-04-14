using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.UserControls
{
    class GenericViewModel : BaseUserControlModel, IViewModel
    {
        #region Private Members

        private GenericView _genericView;

        #endregion

        public GenericViewModel(GenericView genericView)
        {
            _genericView = genericView;
        }
    }
}
