using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MSEACalculator.OtherRes
{
    public class CustomTCommand<T> : ICommand
    {
        readonly Action<T> _action;
        readonly Func<bool> _condition = null;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _condition == null ? true : _condition();
        }

        public void Execute(object parameter)
        {
            this._action((T)parameter);   
        }


        public CustomTCommand(Action<T> action)
        {
            this._action = action;
        }
        public CustomTCommand(Action<T> action, Func<bool> condition)
        {
            if (action == null)
                throw new ArgumentNullException("execute");

            this._action = action;
            this._condition = condition;
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler!= null)
            {
                handler(this, EventArgs.Empty);
            }


            //CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
