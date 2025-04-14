using DiffClient.DataModel;
using DiffClient.UserControls;
using DiffEngine;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

#pragma warning disable 8600
#pragma warning disable 8618

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

        public string PrimaryData { get; internal set; }

        public string SecondaryData { get; internal set; }
    }

    /// <summary>
    /// StatisticsPage.xaml 的交互逻辑
    /// </summary>
    public partial class StatisticsPage : Page
    {
        #region Private Members
        private Frame _indexFrame;
        private MainWindow _mainWindow;
        private StatisticsPageModel _model;
        private void filter_statistics_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (filterAutoComplete.SelectedIndex > 0)
                {
                    filterAutoComplete.SelectedIndex--;
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (filterAutoComplete.SelectedIndex < filterAutoComplete.Items.Count - 1)
                {
                    filterAutoComplete.SelectedIndex++;
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                filter_statistics.Text = _model.FilterCurrent;
                filterAutoComplete.Visibility = Visibility.Collapsed;
                filter_statistics.CaretIndex = filter_statistics.Text.Length;
            }
        }


        private void filterAutoComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(filter_statistics == null) { return; }
        }

        private void filter_statistics_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(filterAutoComplete != null)
            {
                if (string.IsNullOrEmpty(filter_statistics.Text))
                {
                    filterAutoComplete.Visibility = Visibility.Collapsed;
                }
                else
                {
                    filterAutoComplete.Visibility = Visibility.Visible;
                    filterAutoComplete.Items.Filter = item => ((string)item).Contains(filter_statistics.Text, StringComparison.OrdinalIgnoreCase);
                }
            }

            if (_model != null)
            {
                string input = filter_statistics.Text;
                _model.ObservableItems = new FilterTokenizer(input, _model.DiffDecompileItems).Parser();
            }
        }

        private void initContextMenu()
        {
            statisticsDataGrid.ContextMenu = new DataGridColumnInject(new ContextMenuContext()
            {
                MainWindow = _mainWindow,
                DataGrid = statisticsDataGrid,
            }).Register();
        }

        private void statisticsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.ContextMenu = new StatisticsInject(new ContextMenuContext()
            {
                MainWindow = _mainWindow,
                DataGrid = statisticsDataGrid,
                ViewAndViewModelContext = _model,
                indexFrame = _indexFrame
            }).Register();
        }

        private void statisticsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid.SelectedItem == null) return;

            var currow = dataGrid.SelectedItem as DiffDecompileItem;
            _indexFrame.NavigationService.Navigate(
                 new DiffDecompilePreviewPage(_mainWindow, new DiffDecompileArgs()
                 {
                     Title = $"{currow.PrimaryName}-{currow.SecondaryName}",
                     Primary = currow.PrimaryData,
                     Secondary = currow.SecondaryData
                 }));
        }

        private void initBasicInfo()
        {
            oldBox.Children.Clear();
            oldBox.Children.Add(new DiffBasicInfoView(new BasicInfoEntry()
            {
                Header = "Old Version Basic Information",
                Filename = "",
                Arch = "",
                Operation = new List<BasicInfoLinkMetadata>()
                {
                    new BasicInfoLinkMetadata()
                    {
                        Header = "File Download",
                        MsdlLink = ""
                    },
                    new BasicInfoLinkMetadata()
                    {
                        Header = "Patch Download",
                        MsdlLink = ""
                    },
                    new BasicInfoLinkMetadata()
                    {
                        Header = "PDB Download",
                        MsdlLink = ""
                    },
                    new BasicInfoLinkMetadata()
                    {
                        Header = "Associated CVE",
                        IsAssociatedCVE = true,
                        CVEs = new string[] { }
                    },
                }
                
            }));

            newBox.Children.Clear();
            newBox.Children.Add(new DiffBasicInfoView(new BasicInfoEntry()
            {
                Header = "New Version Basic Information",
                Filename = "",
                Arch = "",
                Operation = new List<BasicInfoLinkMetadata>()
                {
                    new BasicInfoLinkMetadata()
                    {
                        Header = "File Download",
                        MsdlLink = ""
                    },
                    new BasicInfoLinkMetadata()
                    {
                        Header = "Patch Download",
                        MsdlLink = ""
                    },
                    new BasicInfoLinkMetadata()
                    {
                        Header = "PDB Download",
                        MsdlLink = ""
                    },
                    new BasicInfoLinkMetadata()
                    {
                        Header = "Associated CVE",
                        IsAssociatedCVE = true,
                        CVEs = new string[] { }
                    },
                }
            }));
        }
        #endregion

        #region Public Members


        #endregion

        public StatisticsPage(MainWindow mainWindow,Frame indexFrame, IEnumerable<DiffDecompileItem> items)
        {
            _mainWindow = mainWindow;
            _indexFrame = indexFrame;
            InitializeComponent();
            if (_model == null)
            {
                _model = new StatisticsPageModel(mainWindow, this);
                _model.InitData(items);
            }
            this.DataContext = _model;
            initContextMenu();
            initBasicInfo();
        }
    }

    /// <summary>
    /// this filter tokenizer class contains  datamodel which is similiary with Wireshark.
    /// </summary>
    public class FilterTokenizer
    {
        #region Private Members

        private const string _contain = "contains: ";
        private const string _starts = "starts: ";
        private const string _contents = "contents: ";
        private const string _and = "&&";
        private const string _or = "||";

        #endregion

        #region Public Members

        public string Token { get; }
        public List<DiffDecompileItem> DiffDecompileItems { get; }

        public bool NoFilter => String.IsNullOrEmpty(Token) || Token == "*";

        public bool UseContainsRule => Token.StartsWith(_contain);
        public bool UseStartRule => Token.StartsWith(_starts);
        public bool UseContentsRule => Token.StartsWith(_contents);

        public ObservableCollection<DiffDecompileItem> Parser()
        {
            if (NoFilter)
            {
                return new ObservableCollection<DiffDecompileItem>(DiffDecompileItems);
            }

            if (UseContainsRule)
            {
                string realfilterstr = Token.Substring(_contain.Length);
                if (!String.IsNullOrEmpty(realfilterstr))
                    return new ObservableCollection<DiffDecompileItem>(DiffDecompileItems.Where(x => x.PrimaryName.Contains(realfilterstr)).ToList());
            }

            if (UseStartRule)
            {
                string realfilterstr = Token.Substring(_starts.Length);
                if (!String.IsNullOrEmpty(realfilterstr))
                    return new ObservableCollection<DiffDecompileItem>(DiffDecompileItems.Where(x => x.PrimaryName.StartsWith(realfilterstr)).ToList());
            }

            if (UseContentsRule)
            {
                string realfilterstr = Token.Substring(_contents.Length);
                if (!String.IsNullOrEmpty(realfilterstr))
                    return new ObservableCollection<DiffDecompileItem>(DiffDecompileItems.Where(x => x.PrimaryData.Contains(realfilterstr) || x.SecondaryData.Contains(realfilterstr)).ToList());
            }

            return new ObservableCollection<DiffDecompileItem>(DiffDecompileItems);
        }

        #endregion

        public FilterTokenizer(string token, List<DiffDecompileItem> diffDecompileItems)
        {
            Token = token;
            DiffDecompileItems = diffDecompileItems;
        }
    }
}
