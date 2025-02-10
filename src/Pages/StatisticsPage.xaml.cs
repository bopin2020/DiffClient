using DiffEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


namespace DiffClient.Pages
{
    public class DiffDecompileItem
    {
        public int Id { get; internal set; }

        public float Similarity { get; internal set; }

        public float Confidence { get; internal set; }

        public string PrimaryName { get; internal set; }

        public long PrimaryAddress { get; internal set; }

        public string SecondaryName { get; internal set; }

        public long SecondaryAddress { get; internal set; }
    }

    /// <summary>
    /// StatisticsPage.xaml 的交互逻辑
    /// </summary>
    public partial class StatisticsPage : Page
    {
        private ObservableCollection<DiffDecompileItem> ObservableItems = new();

        public StatisticsPage(MainWindow mainWindow, IEnumerable<DiffDecompileItem> items)
        {
            InitializeComponent();
            this.DataContext = new StatisticsPageModel(mainWindow,this);
            statisticsDataGrid.ItemsSource = ObservableItems;

            foreach (var item in items)
            {
                ObservableItems.Add(item);
            }
        }
    }
}
