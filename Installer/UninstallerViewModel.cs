using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Installer
{
    public class UninstallerViewModel : Observable
    {
        public ICommand ExitCommand { get; }
        public ICommand InstallCommand { get; }

        bool gameClient;
        public bool GameClient {
            get { return gameClient; }
            set {
                gameClient = value;
                RaisePropertyChanged();

                ValidateInstallerStatus();
            }
        }

        bool gameClientEnabled;
        public bool GameClientEnabled {
            get { return gameClientEnabled; }
            set {
                gameClientEnabled = value;
                RaisePropertyChanged();
            }
        }

        bool legacyGameClient;
        public bool LegacyGameClient {
            get { return legacyGameClient; }
            set {
                legacyGameClient = value;
                RaisePropertyChanged();

                ValidateInstallerStatus();
            }
        }

        bool legacyGameClientEnabled;
        public bool LegacyGameClientEnabled {
            get { return legacyGameClientEnabled; }
            set {
                legacyGameClientEnabled = value;
                RaisePropertyChanged();
            }
        }

        bool isInstalling;
        public bool IsInstalling {
            get { return isInstalling; }
            set {
                isInstalling = value;
                RaisePropertyChanged();
            }
        }

        bool canStartInstallation;
        public bool CanStartInstallation {
            get { return canStartInstallation; }
            set {
                canStartInstallation = value;
                RaisePropertyChanged();
            }
        }

        public UninstallerViewModel() {
            this.ExitCommand = new Command(ExitCommandCallback);
            this.InstallCommand = new Command(InstallCommandCallback);



            ValidateInstallerStatus();
        }

        private void InstallCommandCallback() {
            IsInstalling = true;
            CanStartInstallation = false;

            if (GameClient) {
                UninstallComponent(ComponentDefinitions.Client);
            }

            if (LegacyGameClient) {
                UninstallComponent(ComponentDefinitions.LegacyClient); 
            }

            if (!IsComponentInstalled(InstallationComponent.Client) && !IsComponentInstalled(InstallationComponent.LegacyClient)) {
                RegistryTools.UnregisterInstallation("PMD: Shift!");
            }

            IsInstalling = false;

            GameClient = false;
            LegacyGameClient = false;

            ValidateInstallerStatus();

            MessageBox.Show("Uninstallation complete!");
        }

        private void UninstallComponent(ComponentDefinition componentDefinition) {
            var componentDirectory = GetFullComponentDirectory(componentDefinition.Component);

            // Delete component directory
            try {
                Directory.Delete(componentDirectory, true);
            } catch { }

            // Search for shortcuts
            var desktopShortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), componentDefinition.ShortName + ".lnk");
            if (File.Exists(desktopShortcut)) {
                File.Delete(desktopShortcut);
            }

            var startMenuShortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), componentDefinition.ShortName + ".lnk");
            if (File.Exists(startMenuShortcut)) {
                File.Delete(startMenuShortcut);
            }
        }

        private void ExitCommandCallback() {
            Environment.Exit(0);
        }

        private void ValidateInstallerStatus() {
            var canInstall = true;

            GameClientEnabled = IsComponentInstalled(InstallationComponent.Client);
            LegacyGameClientEnabled = IsComponentInstalled(InstallationComponent.LegacyClient);

            if (!GameClient && !LegacyGameClient) {
                canInstall = false;
            }

            CanStartInstallation = canInstall;
        }

        private string GetFullComponentDirectory(InstallationComponent component) {
            var installationDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var componentDirectory = component.GetInstallationDirectory();

            return Path.Combine(installationDirectory, componentDirectory);
        }

        private bool IsComponentInstalled(InstallationComponent component) {
            var fullComponentDirectory = GetFullComponentDirectory(component);

            if (Directory.Exists(fullComponentDirectory)) {
                return true;
            } else {
                return false;
            }
        }
    }
}
