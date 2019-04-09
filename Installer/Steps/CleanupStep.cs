using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer.Steps
{
    public class CleanupStep : AbstractInstallationStep
    {
        public override Task<StepResult> Execute(InstallationState state) {
            state.UpdateStatus("Cleaning up.");

            if (File.Exists(state.Package.Path)) {
                File.Delete(state.Package.Path);
            }

            return Task.FromResult(StepResult.Success);
        }
    }
}
