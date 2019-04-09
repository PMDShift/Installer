using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer.Steps
{
    public abstract class AbstractInstallationStep
    {
        public abstract Task<StepResult> Execute(InstallationState state);
    }
}
