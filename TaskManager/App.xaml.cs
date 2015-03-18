using System.Windows;
using TaskManager.ViewModels;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow app = new MainWindow();
            TaskManagerViewModel context = new TaskManagerViewModel();
            app.DataContext = context;
            app.Show();
        }
    }
}
