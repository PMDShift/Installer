using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Installer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e) {
            if (e.Args.Where(x => x == "/uninstall").Any()) {
                var window = new UninstallerWindow();
                window.Show();
            } else {
                var window = new MainWindow();
                window.Show();
            }
        }
    }
}
