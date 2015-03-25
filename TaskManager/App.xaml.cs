using System.Windows;
using TaskManager.ViewModels;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(System.IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("User32.dll", EntryPoint = "ShowWindowAsync")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        private const int WS_SHOWNORMAL = 1;

        private static void LaunchSingleInstance() 
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location));
            if (processes.Length > 1)
            {
                System.Diagnostics.Process thisApp = (System.Diagnostics.Process.GetCurrentProcess().Id == processes[0].Id) ? processes[1] : processes[0];
                ShowWindowAsync(thisApp.MainWindowHandle, WS_SHOWNORMAL);
                SetForegroundWindow(thisApp.MainWindowHandle);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            LaunchSingleInstance();

            base.OnStartup(e);

            MainWindow app = new MainWindow();
            TaskManagerViewModel context = new TaskManagerViewModel();
            app.DataContext = context;
            app.Show();
        }
    }
}
