using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.UserControls
{
    internal class GlobalLoggerViewModel : BaseUserControlModel,IViewModel
    {
        #region Private Members

        private GlobalLoggerView globalLoggerView;

        #endregion

        #region Notify Propertys


        #endregion

        public GlobalLoggerViewModel(GlobalLoggerView globalloggerview)
        {
            globalLoggerView = globalloggerview;
        }
    }
}
