using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Installer
{
    public class Command : ICommand
    {
        private readonly Action action;

        public event EventHandler CanExecuteChanged;

        public Command(Action action) {
            this.action = action;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            action();
        }
    }
}
