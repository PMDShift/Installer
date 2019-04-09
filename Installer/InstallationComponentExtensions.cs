using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    public static class InstallationComponentExtensions
    {
        public static string GetInstallationDirectory(this InstallationComponent component) {
            switch (component) {
                case InstallationComponent.Client:
                    return "Client";
                case InstallationComponent.LegacyClient:
                    return "LegacyClient";
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
