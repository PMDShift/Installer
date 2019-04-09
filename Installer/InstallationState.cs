using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    public class InstallationState
    {
        private readonly Action<string> updateStatusAction;
        private readonly Action<int> updateProgressAction;

        public InstallerProcessor Processor { get; }
        public string TargetDirectoryPath { get; }
        public ComponentDefinition ComponentDefinition { get; }
        public InstallationPackage Package { get; set; }

        public InstallationState(InstallerProcessor processor, string installationDirectory, ComponentDefinition componentDefinition, Action<string> updateStatusAction, Action<int> updateProgressAction) {
            this.Processor = processor;
            this.TargetDirectoryPath = Path.Combine(installationDirectory, componentDefinition.InstallationDirectory);
            this.ComponentDefinition = componentDefinition;
            this.updateStatusAction = updateStatusAction;
            this.updateProgressAction = updateProgressAction;
        }

        public void UpdateStatus(string status) {
            updateStatusAction(status);
        }

        public void ReportProgress(int progress) {
            updateProgressAction(progress);
        }
    }
}
