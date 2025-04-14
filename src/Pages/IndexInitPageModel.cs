using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Pages
{
    internal class IndexInitPageModel : BasePageModel<IndexInitPage>
    {
        #region Private Members

        private IndexInitPage _indexInitPage;

        #endregion
        public IndexInitPageModel(MainWindow mainWindow, IndexInitPage view) : base(mainWindow, view)
        {
            _indexInitPage = view;
        }
    }
}
