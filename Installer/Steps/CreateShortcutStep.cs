using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer.Steps
{
    public class CreateShortcutStep : AbstractInstallationStep
    {
        private readonly string targetDirectory;

        public CreateShortcutStep(string targetDirectory) {
            this.targetDirectory = targetDirectory;
        }

        public override Task<StepResult> Execute(InstallationState state) {
            state.UpdateStatus("Creating shortcuts...");

            var iconPath = Path.Combine(state.TargetDirectoryPath, "Icon.ico");

            var wsh = new WshShell();

            var shortcut = wsh.CreateShortcut(Path.Combine(targetDirectory, state.ComponentDefinition.ShortName + ".lnk")) as IWshRuntimeLibrary.IWshShortcut;
            shortcut.TargetPath = Path.Combine(state.TargetDirectoryPath, state.ComponentDefinition.ApplicationFileName);
            shortcut.WindowStyle = 1;
            shortcut.Description = state.ComponentDefinition.Description;
            shortcut.WorkingDirectory = state.TargetDirectoryPath;

            if (System.IO.File.Exists(iconPath)) {
                shortcut.IconLocation = iconPath;
            }

            shortcut.Save();

            return Task.FromResult(StepResult.Success);
        }
    }
}
