using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MSEACalculator.OtherRes
{
    public class CustomCommand : ICommand
    {

        readonly Action _execute = null;
        readonly Func<bool> _canExecute = null;

        public event EventHandler CanExecuteChanged;

        public CustomCommand()
        {

        }

        public CustomCommand(Action execute)
        {
            this._execute = execute;
        }
        
        public CustomCommand(Action execute, Func<bool> canExcute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this._execute = execute;
            this._canExecute = canExcute;
        }


        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();

        }


        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if(handler!= null)
            {
                handler(this, EventArgs.Empty);
            }

            //CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
