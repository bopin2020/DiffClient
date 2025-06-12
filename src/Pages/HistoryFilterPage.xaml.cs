using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using DiffClient.DataModel;

namespace DiffClient.Pages
{
    public partial class HistoryFilterPage : Page
    {
        #region Private Members
        private Window _window;
        private HistoryFilterPageModel _model;
        private Brush _pass = Brushes.LightGreen;
        private Brush _error = Brushes.Red;

        private Dictionary<string, List<string>> autoComplementCache = new Dictionary<string, List<string>>()
        {
            {"root",new List<string>() },
        };

        private List<(string english, string cstyle, string description)> BuiltInVars = new List<(string, string, string)>()
        {
            /*
            filter expression
            https://www.wireshark.org/docs/wsug_html_chunked/ChWorkBuildDisplayFilterSection.html

            english, c-like,description
             */
            ("eq","==","Equal (any if more than one)"),
            ("ne","!=","Not Equal (any if more than one)"),
            ("gt",">","Greater than"),
            ("lt","<","Less than"),
            ("ge",">=","Greater than or equal to"),
            ("le","<=","Less than or equal to"),
            ("true","True","bool value 1"),
            ("false","False","bool value 0"),
            ("and","&&","logic and"),
            ("or","&&","logic or"),
        };

        private void filter_history_KeyDown(object sender, KeyEventArgs e)
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
                filter_history.Text = _model.FilterCurrent;
                filterAutoComplete.Visibility = Visibility.Collapsed;
                filter_history.CaretIndex = filter_history.Text.Length;
            }
            filterAutoComplete.ScrollIntoView(filterAutoComplete.SelectedItem);
        }


        private void filterAutoComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filter_history == null) { return; }
        }

        private void filter_history_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (filterAutoComplete != null)
            {
                if (string.IsNullOrEmpty(filter_history.Text))
                {
                    filterAutoComplete.Visibility = Visibility.Collapsed;
                }
                else
                {
                    filterAutoComplete.Visibility = Visibility.Visible;
                    filterAutoComplete.Items.Filter =
                                        item => ((string)item).Contains(filter_history.Text, StringComparison.OrdinalIgnoreCase)
                                        || filter_history.Text == "*"
                                        || filter_history.Text == "."
                                        ;
                }
            }

            if (_model != null)
            {
                string input = filter_history.Text;
                _model.ObservableItems = new FilterHistoryTokenizer(input, _model.DiffDecompileItems).Parser(out var result);
                if (result.IsError)
                {
                    filter_history.Background = _error;
                }
                else
                {
                    filter_history.Background = _pass;
                }
            }
        }

        private void initContextMenu()
        {

        }

        private void processDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void addProperties()
        {
            foreach (var item in typeof(HistoryEntry).GetProperties())
            {
                _model.AutoCompleteDataSource.Add(item.Name.ToLower());
                autoComplementCache["root"].Add(item.Name.ToLower());
            }
        }

        private void addBuiltinVar()
        {
            foreach (var item in BuiltInVars)
            {
                _model.AutoCompleteDataSource.Add(item.english);
                _model.AutoCompleteDataSource.Add(item.cstyle);
                autoComplementCache["root"].Add(item.english);
                autoComplementCache["root"].Add(item.cstyle);
            }
        }
        #endregion

        #region Public Members

        internal HistoryFilterPageModel HistoryPageModel
        {
            get
            {
                return _model;
            }
        }

        #endregion

        public HistoryFilterPage(Window window,IEnumerable<HistoryEntry> items)
        {
            _window = window;
            InitializeComponent();
            if (_model == null)
            {
                _model = new HistoryFilterPageModel(this);
                _model.InitData(items);
            }
            this.DataContext = _model;
            initContextMenu();
            addProperties();
            addBuiltinVar();
        }

        private void historyDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid.SelectedItem == null) return;

            var currow = dataGrid.SelectedItem as HistoryEntry;

            var view = new MainWindow(currow.Value);
            var viewModel = new MainWindowViewModel(view);
            view.DataContext = viewModel;
            view.Show();

            _window.Close();
        }
    }

    /// <summary>
    /// this filter tokenizer class contains  datamodel which is similiary with Wireshark.
    /// 
    /// true && 1 == 2 && starts: vmp && contains: aaa
    /// rules: pid 
    /// </summary>
    public class FilterHistoryTokenizer
    {
        #region Private Members

        private const string _contain = "contains: ";
        private const string _starts = "starts: ";
        #endregion

        #region Public Members
        public MemoryStream ms;
        public string Token { get; }
        public List<HistoryEntry> DiffDecompileItems { get; }

        public bool NoFilter => String.IsNullOrEmpty(Token)
                                || Token == "*"
                                || Token == "."
                                ;

        public bool UseContainsRule => Token.StartsWith(_contain);
        public bool UseStartRule => Token.StartsWith(_starts);
        /// <summary>
        /// parse token and nexttoken with state machine before  parsing token within filter
        /// </summary>
        /// <param name="tokenizerResult"></param>
        /// <returns></returns>
        public ObservableCollection<HistoryEntry> Parser(out TokenizerResult tokenizerResult)
        {
            tokenizerResult = new TokenizerResult();

            LexerParser();

            if (NoFilter)
            {
                tokenizerResult.IsError = false;
                return new ObservableCollection<HistoryEntry>(DiffDecompileItems);
            }


            if (UseContainsRule)
            {
                string realfilterstr = Token.Substring(_contain.Length);
                Func<HistoryEntry, bool> call = (entry) => { return entry.Value.Contains(realfilterstr); };
                if (!String.IsNullOrEmpty(realfilterstr))
                {
                    tokenizerResult.IsError = false;
                    return new ObservableCollection<HistoryEntry>(DiffDecompileItems.Where(call).ToList());
                }
            }

            if (UseStartRule)
            {
                string realfilterstr = Token.Substring(_starts.Length);
                Func<HistoryEntry, bool> call = (entry) => { return (bool)entry.Value?.StartsWith(realfilterstr); };
                if (!String.IsNullOrEmpty(realfilterstr))
                {
                    tokenizerResult.IsError = false;
                    return new ObservableCollection<HistoryEntry>(DiffDecompileItems.Where(call).ToList());
                }
            }

            tokenizerResult.IsError = true;
            return new ObservableCollection<HistoryEntry>(DiffDecompileItems);
        }

        public List<string> Keywords { get; set; } = new List<string>();

        public bool LexerParser()
        {
            StateObj curstate, substate = StateObj.Init;
            int result = -1;
            do
            {
                int curchr = ms.ReadByte();
                int nexchr = ms.ReadByte();
                if (curchr == -1 || nexchr == -1)
                {
                    break;
                }
                // todo 

                ms.Position--;
                result = curchr;

            } while (result != -1);

            return true;
        }

        #endregion

        public FilterHistoryTokenizer(string token, List<HistoryEntry> diffDecompileItems)
        {
            Token = token;
            ms = new MemoryStream(Encoding.UTF8.GetBytes(Token));
            DiffDecompileItems = diffDecompileItems;
        }
    }
}
