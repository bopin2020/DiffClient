using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

#pragma warning disable

namespace DiffClient.Pages
{
    public class ProcessEntry
    {
        public int Index { get; set; }

        public int Pid { get; set; }

        public int PPid { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }
    }

    internal class ProcessListPageModel : BasePageModel<ProcessListPage>
    {
        #region Notify Propertys

        private ObservableCollection<ProcessEntry> _observable;

        public ObservableCollection<ProcessEntry> ObservableItems
        {
            get
            {
                if (_observable == null)
                {
                    _observable = new ObservableCollection<ProcessEntry>();
                }
                return _observable;
            }

            set
            {
                _observable = value;
                OnPropertyChanged("ObservableItems");
            }
        }

        #endregion

        #region Public Members

        public PlotModel DiffModel { get; private set; }

        public List<ProcessEntry> DiffDecompileItems = new();

        public ObservableCollection<string> AutoCompleteDataSource { get; set; } = new ObservableCollection<string>();

        public string FilterCurrent { get; set; }

        public void InitData(IEnumerable<ProcessEntry> items)
        {
            if (_observable == null)
            {
                _observable = new ObservableCollection<ProcessEntry>();
            }
            _observable.Clear();
            DiffDecompileItems.Clear();

            foreach (var item in items)
            {
                _observable.Add(item);
                DiffDecompileItems.Add(item);
            }
        }

        public void Refresh(IEnumerable<ProcessEntry> items)
        {
            DiffDecompileItems.Clear();

            foreach (var item in items)
            {
                DiffDecompileItems.Add(item);
            }
        }

        #endregion

        public ProcessListPageModel(MainWindow mainWindow, ProcessListPage view) : base(mainWindow, view)
        {
            AutoCompleteDataSource.Add("contains: ");
            AutoCompleteDataSource.Add("starts: ");
        }
    }
}
