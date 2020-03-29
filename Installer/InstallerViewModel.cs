using Installer.Steps;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Installer
{
    public class InstallerViewModel : Observable
    {
        public ICommand ExitCommand { get; }
        public ICommand InstallCommand { get; }
        public ICommand ChooseInstallationDirectoryCommand { get; }

        int installationProgress;
        public int InstallationProgress {
            get { return installationProgress; }
            set {
                installationProgress = value;
                RaisePropertyChanged();
            }
        }

        string installationStatus;
        public string InstallationStatus {
            get { return installationStatus; }
            set {
                installationStatus = value;
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

        bool gameClient;
        public bool GameClient {
            get { return gameClient; }
            set {
                gameClient = value;
                RaisePropertyChanged();
            }
        }

        bool legacyGameClient;
        public bool LegacyGameClient {
            get { return legacyGameClient; }
            set {
                legacyGameClient = value;
                RaisePropertyChanged();
            }
        }

        bool addToStartMenu;
        public bool AddToStartMenu {
            get { return addToStartMenu; }
            set {
                addToStartMenu = value;
                RaisePropertyChanged();
            }
        }

        bool addToDesktop;
        public bool AddToDesktop {
            get { return addToDesktop; }
            set {
                addToDesktop = value;
                RaisePropertyChanged();
            }
        }

        string installationDirectory;
        public string InstallationDirectory {
            get { return installationDirectory; }
            set {
                installationDirectory = value;
                RaisePropertyChanged();

                ValidateInstallerStatus();
            }
        }

        string installationDirectoryError;
        public string InstallationDirectoryError {
            get { return installationDirectoryError; }
            set {
                installationDirectoryError = value;
                RaisePropertyChanged();
            }
        }

        int stepSize;
        int completedSteps;

        List<InstallationStepGroup> installationSteps;

        public InstallerViewModel() {
            this.GameClient = true;

            this.AddToStartMenu = true;
            this.AddToDesktop = true;

            InstallationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PMD Shift");

            ExitCommand = new Command(ExitCommandCallback);
            InstallCommand = new Command(InstallCommandCallback);
            ChooseInstallationDirectoryCommand = new Command(ChooseInstallationDirectoryCommandCallback);

            InstallationStatus = "Waiting to begin...";
        }

        private void ExitCommandCallback() {
            Environment.Exit(0);
        }

        private void ChooseInstallationDirectoryCommandCallback() {
            using (var dialog = new CommonOpenFileDialog()) {
                dialog.IsFolderPicker = true;
                dialog.InitialDirectory = InstallationDirectory;

                var result = dialog.ShowDialog();
                if (result == CommonFileDialogResult.Ok) {
                    this.InstallationDirectory = dialog.FileName;
                }
            }
        }

        private async void InstallCommandCallback() {
            IsInstalling = true;
            CanStartInstallation = false;

            var installerProcessor = new InstallerProcessor();
            installerProcessor.ProgressChanged += InstallerProcessor_ProgressChanged;

            installationSteps = new List<InstallationStepGroup>();

            if (GameClient) {
                var group = new InstallationStepGroup(ComponentDefinitions.Client);

                group.Steps.Add(new DownloadPackageStep());
                group.Steps.Add(new ExtractStep());

                if (AddToDesktop) {
                    group.Steps.Add(new CreateShortcutStep(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
                }
                if (AddToStartMenu) {
                    group.Steps.Add(new CreateShortcutStep(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)));
                }

                group.Steps.Add(new CleanupStep());

                installationSteps.Add(group);
            }

            if (LegacyGameClient) {
                var group = new InstallationStepGroup(ComponentDefinitions.LegacyClient);

                group.Steps.Add(new DownloadPackageStep());
                group.Steps.Add(new ExtractStep());

                if (AddToDesktop) {
                    group.Steps.Add(new CreateShortcutStep(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
                }
                if (AddToStartMenu) {
                    group.Steps.Add(new CreateShortcutStep(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)));
                }

                group.Steps.Add(new CleanupStep());

                installationSteps.Add(group);
            }

            for (var i = 0; i < installationSteps.Count; i++) {
                var group = installationSteps[i];

                var installationState = new InstallationState(installerProcessor,
                                                              InstallationDirectory,
                                                              group.ComponentDefinition,
                                                              status => UpdateStatus(i, installationSteps.Count, status),
                                                              progress => UpdateRealPercentage(progress));

                stepSize = 100 / group.Steps.Count;
                completedSteps = 0;

                foreach (var step in group.Steps) {
                    await step.Execute(installationState);

                    installationState.ReportProgress(100);

                    completedSteps++;
                }
            }

            var installerSource = Assembly.GetEntryAssembly().Location;
            var uninstallerPath = Path.Combine(InstallationDirectory, "Installer.exe");

            InstallationStatus = "Creating uninstaller...";
            if (installerSource != uninstallerPath) {
                File.Copy(installerSource, uninstallerPath, true);
            }

            InstallationStatus = "Registering application...";
            RegistryTools.RegisterInstallation("PMD: Shift!", InstallationDirectory, Path.Combine(InstallationDirectory, "Client", "Icon.ico"), $"\"{uninstallerPath}\" /uninstall");

            InstallationStatus = "Installation complete!";
            IsInstalling = false;

            ValidateInstallerStatus();
        }

        private void UpdateStatus(int groupNumber, int groupTotal, string status) {
            InstallationStatus = $"Component {groupNumber + 1} of {groupTotal}: {status}";
        }

        private void InstallerProcessor_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            UpdateRealPercentage(e.Progress);
        }

        private void UpdateRealPercentage(int stepValue) {
            InstallationProgress = (int)(completedSteps * stepSize) + (int)(stepValue * (stepSize / 100.0f));
        }

        private bool ValidateInstallationDirectory() {
            // Test for write access
            if (!DirectoryTools.CanWriteToDirectory(InstallationDirectory)) {
                InstallationDirectoryError = "Unable to write to directory. Run as an administrator or change directory.";
                return false;
            }

            InstallationDirectoryError = "";

            return true;
        }

        private void ValidateInstallerStatus() {
            var canInstall = true;

            if (!ValidateInstallationDirectory()) {
                canInstall = false;
            }

            CanStartInstallation = canInstall;
        }
    }
}
