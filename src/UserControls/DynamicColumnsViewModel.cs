using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

#pragma warning disable

namespace DiffClient.UserControls
{
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, key);
        }

        public new bool Remove(TKey key)
        {
            if (base.Remove(key))
            {
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, key);
                return true;
            }
            return false;
        }

        private void OnCollectionChanged(NotifyCollectionChangedAction action, TKey key)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, key));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Keys"));
        }
    }

    public class ItemModel
    {
        public ObservableDictionary<string, object> Attributes { get; } = new ObservableDictionary<string, object>();
    }

    internal class DItems
    {
        public string Name { get; set; }

        public int Age { get; set; }
        
        public Dictionary<string,object> ExtensionChildren { get; set; } = new Dictionary<string, object>();

        public void AddEle(string key, object value) 
        {
            if (ExtensionChildren.ContainsKey(key))
            {
                return;
            }
            ExtensionChildren.Add(key, value);
        }
    }

    internal class DynamicColumnsViewModel : INotifyPropertyChanged
    {
        #region Private

        private DynamicColumnsView _dynamicColumnsView;

        private void initColumns()
        {
            foreach (var item in typeof(DItems).GetProperties())
            {
                DynamicKeys.Add(item.Name, null);
            }
        }

        private void initHideColumns()
        {
            var tmp = _dynamicColumnsView.dataGrid.Columns.Where(x => x.Header == "ExtensionChildren").FirstOrDefault();
            if(tmp != null)
                tmp.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Notify

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion


        #region Public

        public ObservableCollection<ItemModel> Items { get; } = new ObservableCollection<ItemModel>();
        public ObservableDictionary<string, object> DynamicKeys { get; } = new ObservableDictionary<string, object>();

        public void AddNewKey(string key)
        {
            DynamicKeys.Add(key, null);
            foreach (var item in Items)
            {
                item.Attributes.Add(key, "");
            }
        }

        public ItemModel QueryTarget()
        {
            return Items.Where(x => x.Attributes.ContainsKey("Name")).FirstOrDefault();
        }

        public void SetValue(ItemModel im, KeyValuePair<string, object> item)
        {
            if (!Items.Contains(im))
            {
                return;
            }

            if (!im.Attributes.ContainsKey(item.Key))
            {
                return;
            }

            im.Attributes[item.Key] = item.Value;
        }


        #endregion
        public DynamicColumnsViewModel(DynamicColumnsView dynamicColumnsView)
        {
            _dynamicColumnsView = dynamicColumnsView;
            initColumns();
            initHideColumns();

            Items.Add(new ItemModel { Attributes = { ["Name"] = "Alice", ["Age"] = 30 } });
            Items.Add(new ItemModel { Attributes = { ["Name"] = "Bob", ["Age"] = 25 } });
        }
    }
}
