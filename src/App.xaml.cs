using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace DiffClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SetupExceptionHandling();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                DiffClient.MainWindow.SetStatusException(((Exception)e.ExceptionObject).Message);

            DispatcherUnhandledException += (s, e) =>
            {
                DiffClient.MainWindow.SetStatusException(e.Exception.Message);
                e.Handled = true;
            };
        }
    }

}
