using DiffClient.Utility;
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

#pragma warning disable

namespace DiffClient.Pages
{
    public partial class ProcessListPage : Page
    {
        #region Private Members

        private ProcessListPageModel _model;
        private MainWindow _mainWindow;
        private Brush _pass = Brushes.LightGreen;
        private Brush _error = Brushes.Red;

        private Dictionary<string, List<string>> autoComplementCache = new Dictionary<string, List<string>>()
        {
            {"root",new List<string>() },
        };

        private List<(string english, string cstyle,string description)> BuiltInVars = new List<(string, string,string)>()
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

        private void filter_process_KeyDown(object sender, KeyEventArgs e)
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
                filter_process.Text = _model.FilterCurrent;
                filterAutoComplete.Visibility = Visibility.Collapsed;
                filter_process.CaretIndex = filter_process.Text.Length;
            }
            filterAutoComplete.ScrollIntoView(filterAutoComplete.SelectedItem);
        }


        private void filterAutoComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filter_process == null) { return; }
        }

        private void filter_process_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (filterAutoComplete != null)
            {
                if (string.IsNullOrEmpty(filter_process.Text))
                {
                    filterAutoComplete.Visibility = Visibility.Collapsed;
                }
                else
                {
                    filterAutoComplete.Visibility = Visibility.Visible;
                    filterAutoComplete.Items.Filter =
                                        item => ((string)item).Contains(filter_process.Text, StringComparison.OrdinalIgnoreCase)
                                        || filter_process.Text == "*"
                                        || filter_process.Text == "."
                                        ;
                }
            }

            if (_model != null)
            {
                string input = filter_process.Text;
                _model.ObservableItems = new FilterProcessTokenizer(input, _model.DiffDecompileItems).Parser(out var result);
                if (result.IsError)
                {
                    filter_process.Background = _error;
                }
                else 
                {
                    filter_process.Background = _pass;
                }
            }
        }

        private void initContextMenu()
        {
            processDataGrid.ContextMenu = new DataGridColumnInject(new ContextMenuContext()
            {
                MainWindow = _mainWindow,
                DataGrid = processDataGrid,
            }).Register();
        }

        private void processDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.ContextMenu = new ProcessInject(new ContextMenuContext()
            {
                MainWindow = _mainWindow,
                DataGrid = processDataGrid,
                ViewAndViewModelContext = ProcessPageModel
            }).Register();
        }

        private void addProperties()
        {
            foreach (var item in typeof(ProcessEntry).GetProperties())
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
        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessPageModel.Refresh(Help.GetProcesses());
            _model.ObservableItems = new FilterProcessTokenizer(filter_process.Text, _model.DiffDecompileItems).Parser(out var result);
        }

        #endregion

        #region Public Members

        internal ProcessListPageModel ProcessPageModel
        {
            get
            {
                return _model;
            }
        }

        #endregion

        public ProcessListPage(MainWindow mainWindow, IEnumerable<ProcessEntry> items)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            if (_model == null)
            {
                _model = new ProcessListPageModel(mainWindow, this);
                _model.InitData(items);
            }
            this.DataContext = _model;
            initContextMenu();
            addProperties();
            addBuiltinVar();
        }
    }

    public class TokenizerResult
    {
        public bool IsComplement { get; set; }

        public bool IsError { get; set; }
    }

    internal enum StateObj
    {
        Init,
        EnterSubState,
        ExitSubState,
        FieldLexer,
        End,
    }

    /// <summary>
    /// this filter tokenizer class contains  datamodel which is similiary with Wireshark.
    /// 
    /// true && 1 == 2 && starts: vmp && contains: aaa
    /// rules: pid 
    /// </summary>
    public class FilterProcessTokenizer
    {
        #region Private Members

        private const string _contain = "contains: ";
        private const string _starts = "starts: ";
        #endregion

        #region Public Members
        public MemoryStream ms;
        public string Token { get; }
        public List<ProcessEntry> DiffDecompileItems { get; }

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
        public ObservableCollection<ProcessEntry> Parser(out TokenizerResult tokenizerResult)
        {
            tokenizerResult = new TokenizerResult();

            LexerParser();

            if (NoFilter)
            {
                tokenizerResult.IsError = false;
                return new ObservableCollection<ProcessEntry>(DiffDecompileItems);
            }


            if (UseContainsRule)
            {
                string realfilterstr = Token.Substring(_contain.Length);
                Func<ProcessEntry, bool> call = (entry) => { return entry.FullName.Contains(realfilterstr) || entry.Name.Contains(realfilterstr); };
                if (!String.IsNullOrEmpty(realfilterstr))
                {
                    tokenizerResult.IsError = false;
                    return new ObservableCollection<ProcessEntry>(DiffDecompileItems.Where(call).ToList());
                }
            }

            if (UseStartRule)
            {
                string realfilterstr = Token.Substring(_starts.Length);
                Func<ProcessEntry, bool> call = (entry) => { return (bool)entry.FullName?.StartsWith(realfilterstr) || entry.Name.StartsWith(realfilterstr); };
                if (!String.IsNullOrEmpty(realfilterstr))
                {
                    tokenizerResult.IsError = false;
                    return new ObservableCollection<ProcessEntry>(DiffDecompileItems.Where(call).ToList());
                }
            }

            tokenizerResult.IsError = true;
            return new ObservableCollection<ProcessEntry>(DiffDecompileItems);
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
                if(curchr == -1 || nexchr == -1)
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

        public FilterProcessTokenizer(string token, List<ProcessEntry> diffDecompileItems)
        {
            Token = token;
            ms = new MemoryStream(Encoding.UTF8.GetBytes(Token));
            DiffDecompileItems = diffDecompileItems;
        }
    }
}
