using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TemplateFoundation.Validation.Interfaces;

namespace TemplateFoundation.Validation.Implementations
{
    public class ValidatableObject<T> : IValidatable<T>
    {
        public bool CleanOnChange { get; set; } = true;

        T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;

                if (CleanOnChange)
                    IsValid = true;
            }
        }

        public List<IValidationRule<T>> ValidationRules => new List<IValidationRule<T>>();

        public List<string> Errors { get; set; } = new List<string>();
        public bool IsValid { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = ValidationRules.Where(v => !v.Check(Value)).Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return this.IsValid;
        }
        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
