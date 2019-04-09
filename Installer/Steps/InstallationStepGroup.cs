using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer.Steps
{
    public class InstallationStepGroup
    {
        public List<AbstractInstallationStep> Steps { get; }
        public ComponentDefinition ComponentDefinition { get; }

        public InstallationStepGroup(ComponentDefinition componentDefinition) {
            this.Steps = new List<AbstractInstallationStep>();
            this.ComponentDefinition = componentDefinition;
        }
    }
}
