using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TemplateFoundation.Commands
{
    public class RelayCommandWithArgsAsync<T> : BaseCommand
    {
        public RelayCommandWithArgsAsync(Func<T, Task> action)
        {
            this.actionExecute = action;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            try
            {
                await this.actionExecute.Invoke((T)parameter);
                this.SuccessCommand = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                this.SuccessCommand = false;
            }
        }

        public override void Execute(object parameter)
        {
            try
            {
                this.actionExecute.Invoke((T)parameter);
                this.SuccessCommand = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                this.SuccessCommand = false;
            }
        }

        private readonly Func<T, Task> actionExecute;
    }
}
