using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace DiffClient.Pages
{
    public class HistoryEntry
    {
        public string Value { get; set; }
    }

    internal class HistoryFilterPageModel : BasePageModel<HistoryFilterPage>
    {
        #region Notify Propertys

        private ObservableCollection<HistoryEntry> _observable;

        public ObservableCollection<HistoryEntry> ObservableItems
        {
            get
            {
                if (_observable == null)
                {
                    _observable = new ObservableCollection<HistoryEntry>();
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

        public List<HistoryEntry> DiffDecompileItems = new();

        public ObservableCollection<string> AutoCompleteDataSource { get; set; } = new ObservableCollection<string>();

        public string FilterCurrent { get; set; }

        public void InitData(IEnumerable<HistoryEntry> items)
        {
            if (_observable == null)
            {
                _observable = new ObservableCollection<HistoryEntry>();
            }
            _observable.Clear();
            DiffDecompileItems.Clear();

            foreach (var item in items)
            {
                _observable.Add(item);
                DiffDecompileItems.Add(item);
            }
        }

        public void Refresh(IEnumerable<HistoryEntry> items)
        {
            DiffDecompileItems.Clear();

            foreach (var item in items)
            {
                DiffDecompileItems.Add(item);
            }
        }

        #endregion

        public HistoryFilterPageModel(HistoryFilterPage view) : base(null, view)
        {
            AutoCompleteDataSource.Add("contains: ");
            AutoCompleteDataSource.Add("starts: ");
        }
    }
}
