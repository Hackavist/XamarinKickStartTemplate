using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BaseTemplate.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        //The id is set to int just for the simplicity of the demo
        public int Id { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}