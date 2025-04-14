using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

#pragma warning disable

namespace DiffClient.Pages
{
    /// <summary>
    /// ChooseColumns.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseColumns : Page
    {
        private DataGrid _dataGrid;

        private void initPanel()
        {
            panel.Children.Clear();
            foreach (var item in _dataGrid.Columns)
            {
                var rb = new CheckBox()
                {
                    Content = item.Header.ToString(),
                    IsChecked = item.Visibility != Visibility.Hidden ? true : false,
                    Margin = new Thickness(5)
                };
                rb.Checked += Rb_Checked;
                rb.Unchecked += Rb_Unchecked;

                panel.Children.Add(rb);
            }
        }

        private void Rb_Unchecked(object sender, RoutedEventArgs e)
        {
            var tmp = _dataGrid.Columns.Where(x => x.Header.ToString() == ((CheckBox)sender).Content).FirstOrDefault();
            if(tmp != null)
            {
                tmp.Visibility = Visibility.Collapsed;
            }
        }

        private void Rb_Checked(object sender, RoutedEventArgs e)
        {
            var tmp = _dataGrid.Columns.Where(x => x.Header.ToString() == ((CheckBox)sender).Content).FirstOrDefault();
            if (tmp != null)
            {
                tmp.Visibility = Visibility.Visible;
            }
        }

        public ChooseColumns(DataGrid dataGrid)
        {
            _dataGrid = dataGrid;
            InitializeComponent();
            initPanel();
        }
    }
}
