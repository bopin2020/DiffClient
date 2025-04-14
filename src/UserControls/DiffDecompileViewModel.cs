using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.UserControls
{
    class DiffDecompileViewModel : BaseUserControlModel, IViewModel
    {
        #region Private Members

        private DiffDecompile _decompile;

        #endregion

        public DiffDecompileViewModel(DiffDecompile decompile)
        {
            _decompile = decompile;
        }
    }
}
