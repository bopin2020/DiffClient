using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Pages
{
    internal class AutoComplementBindingBuild<T> : INotifyPropertyChanged
        where T : class // T remains data entry
    {
        #region Private

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Notify Members

        private ObservableCollection<T> _observable;

        public ObservableCollection<T> ObservableItems
        {
            get
            {
                if (_observable == null)
                {
                    _observable = new ObservableCollection<T>();
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

        #region Protected

        public List<T> AutoCompleteDataSourceFull = new();
        /// <summary>
        /// default  autocomplement use string
        /// </summary>
        public ObservableCollection<string> AutoCompleteDataSource { get; set; } = new ObservableCollection<string>();

        #endregion

        #region Public

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        public AutoComplementBindingBuild()
        {
            if (_observable == null)
            {
                _observable = new ObservableCollection<T>();
            }
        }
    }
}
