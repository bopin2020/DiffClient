using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

#pragma warning disable

namespace DiffClient.UserControls
{
    internal class IndexPageViewModel : BaseUserControlModel,IViewModel
    {
        #region Private

        private IndexPageView _indexPageView;

        #endregion

        #region Notify

        private ObservableCollection<TreeViewItem> _nodes;
        public ObservableCollection<TreeViewItem> TreeItemsSource
        {
            get => _nodes;
            set
            {
                _nodes = value;
                OnPropertyChanged("TreeItemsSource");
            }
        }

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                using (var tvf = new TreeViewFilter(TreeItemsSource, FilterText))
                {
                    tvf.FilterNodes();
                }
            }
        }

        #endregion

        #region Public



        #endregion



        public IndexPageViewModel(IndexPageView indexPageView) 
        {
            _indexPageView = indexPageView;
            _indexPageView.ViewModel = this;

            TreeItemsSource = new ObservableCollection<TreeViewItem>()
            {
            };
        }
    }
    /// <summary>
    /// we need filter to be more smarter
    /// </summary>
    internal class TreeViewFilter : IDisposable
    {
        private ObservableCollection<TreeViewItem> treeItemsSource;
        private string filterText;
        private bool disposedValue;

        public TreeViewFilter(ObservableCollection<TreeViewItem> TreeItemsSource,string FilterText)
        {
            treeItemsSource = TreeItemsSource;
            filterText = FilterText;
        }

        public void FilterNodes()
        {
            foreach (var item in treeItemsSource)
            {
                (item as DiffTreeItem).IsShowParent = false;
                FilterNode((item as DiffTreeItem), filterText);
            }
        }

        public void FilterNode(DiffTreeItem item, string filterText)
        {
            if (item.Children.Count > 0)
            {
                foreach (var child in item.Children)
                {
                    if (item.Type == TreeItemType.LocalRoot)
                    {
                        item.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        item.Visibility = string.IsNullOrEmpty(filterText)
                            || item.Header.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase)
                            ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    }
                    child.Visibility = child.Header.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase)
                                    ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    if(child.Visibility == System.Windows.Visibility.Visible)
                    {
                        item.Visibility = child.Visibility;
                    }

                    child.IsShowParent = true;
                    FilterNode(child, filterText);
                }
            }

            item.Visibility = string.IsNullOrEmpty(filterText)
                               || item.Header.ToString().Contains(filterText, StringComparison.OrdinalIgnoreCase)
                               ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;

            if (item.IsShowParent)
            {
                item.Visibility = System.Windows.Visibility.Visible;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~TreeViewFilter()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
