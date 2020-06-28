using System.Collections.Generic;
using System.ComponentModel;

namespace TemplateFoundation.Validation.Interfaces
{
    public interface IValidatable<T> : INotifyPropertyChanged
    {
        List<IValidationRule<T>> ValidationRules { get; }

        List<string> Errors { get; set; }

        bool Validate();

        bool IsValid { get; set; }
    }
}
