using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XrayKunden.ViewModel
{
    public class DelegateCommand : ICommand
    {
        readonly Action<object>? execute;
        readonly Predicate<object>? canExecute;


        public DelegateCommand(Action<object>? execute, Predicate<object>? canExecute) // Konstruktor 1
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public DelegateCommand(Action<object>? execute) : this(execute, null) { }  // dieser Konstruktor ruft Konstruktor 1 auf mit this (canExecute ist null)!!



        public event EventHandler? CanExecuteChanged;
        public void RaiseCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object? parameter) => this.canExecute?.Invoke(parameter) ?? true;


        public void Execute(object? parameter) => this.execute?.Invoke(parameter);

    }
}
