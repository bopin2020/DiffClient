using System;
using System.Collections.Generic;
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

namespace DiffClient.UserControls
{
    /// <summary>
    /// GloablLoggerView.xaml 的交互逻辑
    /// </summary>
    public partial class GlobalLoggerView : UserControl
    {
        #region Private

        private MainWindow _mainWindow;
        private void richtextbox_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Hyperlink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void initContextMenu()
        {
            richtextbox.ContextMenu = new MenuInjectConsole(new ContextMenuContext()
                                                        {
                                                            MainWindow = _mainWindow,
                                                            RichTextBox = richtextbox
                                                        }).Register();
        }

        #endregion

        public GlobalLoggerView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            var vm = _mainWindow.mainWindowViewModel;
            vm.GlobalLogStream = new FileStream(Path.Combine(vm.DiffClientWorkDir.FullName,Guid.NewGuid().ToString().Substring(0,8) + ".log"),FileMode.OpenOrCreate,FileAccess.ReadWrite);
            InitializeComponent();
            initContextMenu();
        }

        ~GlobalLoggerView()
        {
            _mainWindow.mainWindowViewModel.GlobalLogStream.Flush();
            _mainWindow.mainWindowViewModel.GlobalLogStream.Dispose();
            _mainWindow.mainWindowViewModel.GlobalLogStream.Close();
        }
    }
}
