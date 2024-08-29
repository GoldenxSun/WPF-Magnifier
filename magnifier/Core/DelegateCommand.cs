using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace magnifier.Core
{
    internal class DelegateCommand<T> : ICommand
    {
        readonly Action<T>? _execute;
        readonly Predicate<T>? _canExecute;

        public DelegateCommand(Predicate<T>? canExecute, Action<T>? execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public DelegateCommand(Action<T> execute) : this(null, execute) { }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object? parameter) => this._canExecute?.Invoke((T)parameter!) ?? true;

        public void Execute(object? parameter) => this._execute?.Invoke((T)parameter!);
    }
}
