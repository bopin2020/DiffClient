using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.UserControls
{
    internal class PerformanceViewModel : BaseUserControlModel,IViewModel
    {
        private PerformanceView _performanceView;
        public PerformanceViewModel(PerformanceView performanceView)
        {
            _performanceView = performanceView;
        }
    }
}
