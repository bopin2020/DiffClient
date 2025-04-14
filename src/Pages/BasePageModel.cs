using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Pages
{
    internal abstract class BasePageModel<T> : INotifyPropertyChanged
    {
        #region Private Members

        private T _curView;

        #endregion

        #region Internal Members

        protected MainWindow _mainWindow { get; set; }

        #endregion

        #region Notify Impl

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Public Members

        public T ViewValue
        {
            get
            {
                return _curView;
            }
        }

        #endregion

        protected BasePageModel(MainWindow mainWindow,T view)
        {
            _mainWindow = mainWindow;
            _curView = view;
        }
    }
}
