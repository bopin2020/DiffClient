using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Windows
{
    internal class HistoryWindowViewModel : BaseWindowNotifyModel
    {
        #region Private Members

        private HistoryWindow _historyWindow;

        #endregion

        #region Public Members
        public string New { get; set; } =  "New";
        public string New_Desc { get; set; } =  "Diff a new file";
        public string Go { get; set; } =  "Go";
        public string Go_Desc { get; set; } =  "Work on your own";
        public string Previous { get; set; } =  "Previous";
        public string Previous_Desc { get; set; } =  "Load the old diffdecompile";

        #endregion
        public HistoryWindowViewModel(HistoryWindow historyWindow)
        {
            _historyWindow =  historyWindow;
        }
    }
}
