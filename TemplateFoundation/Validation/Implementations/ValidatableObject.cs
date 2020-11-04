using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TemplateFoundation.Validation.Interfaces;

namespace TemplateFoundation.Validation.Implementations
{
    public class ValidatableObject<T> : IValidatable<T>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<IValidationRule<T>> ValidationRules { get; } = new List<IValidationRule<T>>();

        public List<string> Errors { get; set; } = new List<string>();

        public bool IsValid { get; set; } = false;

        public string ErrorMsg { get; set; }

        private readonly Action onChange;

        public ValidatableObject(Action action = null)
        {
            onChange = action;
        }

        T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                IsValid = Validate();
                onChange?.Invoke();
            }
        }

        public virtual bool Validate()
        {
            Errors.Clear();
            IEnumerable<string> errors = ValidationRules.Where(v => !v.Check(Value)).Select(v => v.ValidationMessage);
            Errors = errors.ToList();
            ErrorMsg = Errors.FirstOrDefault();
            IsValid = !Errors.Any();
            return this.IsValid;
        }
        public override string ToString() => $"{Value}";
    }
}
