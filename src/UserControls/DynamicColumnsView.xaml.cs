using RichTextBoxLib;
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

namespace DiffClient.UserControls
{
    /// <summary>
    /// DynamicColumns.xaml 的交互逻辑
    /// </summary>
    public partial class DynamicColumnsView : UserControl
    {
        #region Private

        private MainWindow _mainWindow;
        private DynamicColumnsViewModel _viewModel;

        private void Richtextbox_PreviewDrop(object sender, DragEventArgs e)
        {

        }
        private void Richtextbox_PreviewDragEnter(object sender, DragEventArgs e)
        {

        }
        private void Hyperlink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("ok");
        }
        private void DynamicKeys_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GenerateColumns();
        }
        private void GenerateColumns()
        {
            dataGrid.Columns.Clear();
            var vm = DataContext as DynamicColumnsViewModel;
            if (vm == null) return;

            foreach (var key in vm.DynamicKeys.Keys)
            {
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = key,
                    Binding = new Binding($"Attributes[{key}]")
                });
            }
        }

        private void richtextbox_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void init()
        {
            ParagraphHyperlink pl = new ParagraphHyperlink();
            var root = new HyperlinkObj("@RootName", "根节点", null);
            richtextbox.Document.Blocks.Add(pl.Add(root));
            var pp = new HyperlinkObj("Module", "模块", root);
            pp.Children.Add(new HyperlinkObj("SubModule1", "模块1", pp));
            pp.Children.Add(new HyperlinkObj("SubModule2", "模块2", pp));
            pp.Children.Add(new HyperlinkObj("SubModule3", "模块3", pp));

            richtextbox.Document.Blocks.Add(pl.Add(pp));

            richtextbox.Document.Blocks.Add(pl.Add(new HyperlinkObj("Processes", "进程", root)));
            richtextbox.Document.Blocks.Add(pl.Add(new HyperlinkObj("Threads", "线程", root)));
            richtextbox.Document.Blocks.Add(pl.Add(new HyperlinkObj("Sessions", "会话", root)));
        }

        private void input_dynamic_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void input_dynamic_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void filter_dynamic_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void filter_dynamic_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void initContextMenu()
        {
            richtextbox.ContextMenu = new MenuInjectConsole(new ContextMenuContext() { MainWindow = _mainWindow }).Register();
            dataGrid.ContextMenu = new DataGridColumnInject(new ContextMenuContext()
            {
                MainWindow = _mainWindow,
                DataGrid = dataGrid,
            }).Register();
        }

        #endregion

        public DynamicColumnsView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            richtextbox.PreviewDragEnter += Richtextbox_PreviewDragEnter;
            richtextbox.PreviewDragOver += Richtextbox_PreviewDragEnter;
            richtextbox.PreviewDrop += Richtextbox_PreviewDrop;
            richtextbox.AllowDrop = true;
            _viewModel = new DynamicColumnsViewModel(this);
            this.DataContext = _viewModel;
            ((DynamicColumnsViewModel)this.DataContext).DynamicKeys.CollectionChanged += DynamicKeys_CollectionChanged;

            for (int i = 0;i < 10;i++)
            {
                ((DynamicColumnsViewModel)this.DataContext).AddNewKey(i.ToString());
            }

            _viewModel.SetValue(_viewModel.QueryTarget(),new KeyValuePair<string, object>("0", "bopin"));

            init();
            initContextMenu();
        }
    }
}
