using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TemplateFoundation.Commands
{
    public class RelayCommandWithArgsAsync<T> : BaseCommand
    {
        public RelayCommandWithArgsAsync(Func<T, Task> action)
        {
            _actionExecute = action;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await _actionExecute.Invoke((T)parameter);
                SuccessCommand = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                SuccessCommand = false;
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                _actionExecute.Invoke((T)parameter);
                SuccessCommand = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                SuccessCommand = false;
            }
        }

        private readonly Func<T, Task> _actionExecute;
    }
}
