using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    public class InstallationPackage
    {
        public InstallationComponent Component { get; }
        public string Path { get; }

        public InstallationPackage(InstallationComponent component, string path) {
            this.Component = component;
            this.Path = path;
        }
    }
}
