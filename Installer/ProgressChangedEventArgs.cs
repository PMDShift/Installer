using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Installer
{
    public class ProgressChangedEventArgs : EventArgs
    {
        public int Progress { get; }

        public ProgressChangedEventArgs(int progress) {
            this.Progress = progress;
        }
    }
}
