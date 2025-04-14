using DiffClient.DataModel;
using DiffClient.Pages;
using DiffClient.UserControls;
using DiffClient.Utility;
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
using System.Windows.Shapes;

#pragma warning disable

namespace DiffClient
{
    /// <summary>
    /// DiffDecompile.xaml 的交互逻辑
    /// </summary>
    public partial class DiffDecompile : UserControl
    {
        #region Private Members

        private string searchRange { get; set; }
        private MainWindow mainWindow;

        #endregion

        public DiffDecompile(DiffDecompileArgs item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("item");
            }
            InitializeComponent();
            MainWindow.SetStatusException($"{nameof(DiffDecompile)}.SetDiffModel", LogStatusLevel.Info);
            indexFrame.NavigationService.Navigate(new DiffDecompilePreviewPage(mainWindow, new DiffDecompileArgs()
                                         {
                                             Primary = item.Primary,
                                             Secondary = item.Secondary,

                                         }));
        }
    }
}
