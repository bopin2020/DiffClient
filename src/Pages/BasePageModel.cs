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
        protected MainWindow _mainWindow { get; set; }
        private T _curView;
        protected BasePageModel(MainWindow mainWindow,T view)
        {
            _mainWindow = mainWindow;
            _curView = view;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
