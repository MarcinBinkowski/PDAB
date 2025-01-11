using System.Configuration;
using System.Data;
using System.Windows;

namespace PDAB
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Console.WriteLine("Application Starting...");
        
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
