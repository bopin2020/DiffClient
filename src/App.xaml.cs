using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using DiffClient.Windows;
using DiffDecompile.Core;

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
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1] == "-noui")
            {
                var diffd = new DiffDecompileManager();
                var results = diffd.ParseFromFile(args[2]);
                foreach (var result in results)
                {
                    Console.WriteLine($"{result.Id} {result.PrimaryName} {result.SecondaryName}");
                }
                Environment.Exit(0);
            }

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
