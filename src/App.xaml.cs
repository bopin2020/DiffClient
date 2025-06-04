using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using DiffClient.Windows;

namespace DiffClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.All;
            SetupExceptionHandling();
        }

        private void SetupExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                DiffClient.MainWindow.SetStatusException(((Exception)e.ExceptionObject).Message);
            };

            DispatcherUnhandledException += (s, e) =>
            {
                DiffClient.MainWindow.SetStatusException(e.Exception.Message);
                e.Handled = true;
                MainWindowViewModel.PushStrWithGuard(e.Exception.StackTrace + "\n\n\n" + e.Exception.Message);
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (Environment.GetCommandLineArgs().Length == 2)
            {
                var view = new MainWindow();
                var viewModel = new MainWindowViewModel(view);
                view.DataContext = viewModel;
                view.ShowDialog();
            }
            else
            {
                var hw = new HistoryWindow();
                hw.DataContext = new HistoryWindowViewModel(hw);
                hw.ShowDialog();
            }
        }
    }
}
