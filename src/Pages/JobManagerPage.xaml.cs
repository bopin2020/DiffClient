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
using DiffClient.UserControls;
using RichTextBoxLib;

namespace DiffClient.Pages
{
    /// <summary>
    /// JobManagerPage.xaml 的交互逻辑
    /// </summary>
    public partial class JobManagerPage : Page
    {
        #region Private
        private MainWindow _mainWindow;
        private JobManagerPageModel _jobmanagerPageModel;

        private void DynamicKeys_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GenerateColumns();
        }
        private void GenerateColumns()
        {
            jobGrid.Columns.Clear();
            var vm = DataContext as JobManagerPageModel;
            if (vm == null) return;

            foreach (var key in vm.DynamicKeys.Keys)
            {
                //if (key == "JobStatus" || key == "JobLevel")
                //{
                //    jobGrid.Columns.Add(new DataGridComboBoxColumn
                //    {
                //        Header = key,
                //        // todo
                //        SelectedValueBinding = new Binding($"Attributes[{key}]")
                //    });
                //}
                jobGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = key,
                    Binding = new Binding($"Attributes[{key}]")
                });
            }
        }
        private void initContextMenu()
        {
            jobGrid.ContextMenu = new DataGridColumnInject(new ContextMenuContext()
            {
                MainWindow = _mainWindow,
                DataGrid = jobGrid,
                ViewAndViewModelContext = _jobmanagerPageModel
            }).Register();
        }
        private void jobGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.ContextMenu = new JobManagerInject(new ContextMenuContext()
            {
                MainWindow = _mainWindow,
                DataGrid = jobGrid,
                ViewAndViewModelContext = _jobmanagerPageModel
            }).Register();
        }

        private void jobfilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            // todo
            ((Paragraph)jobfilter.Document.Blocks.ToList()[0]).Inlines.ToArray();
        }

        #endregion
        public JobManagerPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _jobmanagerPageModel = new JobManagerPageModel(this);
            this.DataContext = _jobmanagerPageModel;
            ((JobManagerPageModel)this.DataContext).DynamicKeys.CollectionChanged += DynamicKeys_CollectionChanged;
            initContextMenu();
            // todo 
            ((JobManagerPageModel)this.DataContext).AddNewKey("IsAsync");
            _jobmanagerPageModel.SetValue(_jobmanagerPageModel.QueryTarget(), new KeyValuePair<string, object>("0", "bopin"));

        }

        internal JobManagerPageModel JobmanagerPageModel
        {
            get { return _jobmanagerPageModel; }
        }
    }
}
