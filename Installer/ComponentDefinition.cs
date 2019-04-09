using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    public class ComponentDefinition
    {
        public InstallationComponent Component { get; }
        public string InstallationDirectory { get; }
        public string ApplicationFileName { get; }
        public string Description { get; }
        public string ShortName { get; }

        public ComponentDefinition(InstallationComponent component, string installationDirectory, string applicationFileName, string shortName, string description) {
            this.Component = component;
            this.InstallationDirectory = installationDirectory;
            this.ApplicationFileName = applicationFileName;
            this.ShortName = shortName;
            this.Description = description;
        }
    }
}
