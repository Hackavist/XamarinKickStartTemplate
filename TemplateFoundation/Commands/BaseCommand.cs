using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TemplateFoundation.Commands
{
    public class BaseCommand : ICommand
    {
        public bool Succes => SuccessCommand;

        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
        }

        public virtual async Task ExecuteAsync(object parameter)
        {
            await Task.FromResult(0);
        }

        protected bool SuccessCommand;
    }
}
