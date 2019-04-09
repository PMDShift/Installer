using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer.Steps
{
    public class DownloadPackageStep : AbstractInstallationStep
    {
        public override async Task<StepResult> Execute(InstallationState state) {
            state.UpdateStatus("Downloading package...");

            var package = await state.Processor.DownloadPackage(state.ComponentDefinition.Component);

            if (package != null) {
                state.Package = package;

                return StepResult.Success;
            } else {
                return StepResult.Failure;
            }
        }
    }
}
