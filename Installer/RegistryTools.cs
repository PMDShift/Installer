using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    public class RegistryTools
    {
        // https://www.morgantechspace.com/2013/09/add-or-remove-control-panel-programs-in.html
        public static void RegisterInstallation(string componentName, string installDirectory, string iconPath, string uninstallString) {
            string registryPath = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

            var hKey = Registry.CurrentUser.OpenSubKey(registryPath, true);
            var appKey = hKey.CreateSubKey(componentName);

            appKey.SetValue("DisplayName", componentName, RegistryValueKind.String);
            appKey.SetValue("Publisher", "PMD: Shift! Team", RegistryValueKind.String);
            appKey.SetValue("InstallLocation", installDirectory, RegistryValueKind.ExpandString);
            appKey.SetValue("DisplayIcon", iconPath, RegistryValueKind.String);
            appKey.SetValue("UninstallString", uninstallString, RegistryValueKind.ExpandString);
            appKey.SetValue("DisplayVersion", "v1.0", RegistryValueKind.String);
        }

        // https://www.morgantechspace.com/2013/09/add-or-remove-control-panel-programs-in.html
        public static void UnregisterInstallation(string componentName) {
            string registryPath = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

            var homeKey = Registry.CurrentUser.OpenSubKey(registryPath, true);
            var appSubKey = homeKey.OpenSubKey(componentName);

            if (appSubKey != null) {
                homeKey.DeleteSubKey(componentName);
            }
        }
    }
}
