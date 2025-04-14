using DiffClient.Commands;
using DiffClient.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#pragma warning disable

namespace DiffClient.DataModel
{
    internal class EnhancedTabItem<TV,TVM> : TabItem
    {
        private ICommand _command;

        protected Image image = new Image()
        {
            MinWidth = 10,
            MaxWidth = 10,
            MinHeight = 10,
            MaxHeight = 10,
            Source = new BitmapImage(new Uri(@"./resources/close.png", UriKind.Relative))
        };

        public EnhancedTabItem(MainWindow mainWindow, string title,object args = null)
        {
            _command = new EnhancedTabItemCommand(mainWindow);

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(new TextBlock { Text = title });
            sp.Children.Add(new Button() { Content = image, Command = _command, CommandParameter = this });

            Header = sp;
            Background = Brushes.White;
            Content = args != null ? Activator.CreateInstance(typeof(TV), args) : Activator.CreateInstance(typeof(TV));
            DataContext = Activator.CreateInstance(typeof(TVM),Content);
        }
    }
}
