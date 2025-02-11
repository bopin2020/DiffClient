using DiffClient.Commands;
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
using System.Windows.Shapes;

namespace DiffClient.Windows
{
    /// <summary>
    /// ExitDialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ExitDialogWindow : Window
    {
        private ExitCommand _exitCommand;
        public ExitDialogWindow(ExitCommand exitCommand)
        {
            InitializeComponent();
            this.Title = "Save database";
            _exitCommand = exitCommand;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _exitCommand.Cancel = true;
            this.Close();
        }

        private void NotStore_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            _exitCommand.Cancel = false;
            this.Close();
        }
    }
}
