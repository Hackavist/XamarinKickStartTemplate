using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CoreServices.ValidationService.Interfaces;

namespace CoreServices.ValidationService.Implementations
{
	public class ValidatableObject<T> : IValidatable<T>
	{
		public string ErrorMsg { get; set; }

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

		private readonly Action onChange;

		private T _value;

		public ValidatableObject(Action action = null)
		{
			onChange = action;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public List<IValidationRule<T>> ValidationRules { get; } = new List<IValidationRule<T>>();

		public List<string> Errors { get; set; } = new List<string>();

		public bool IsValid { get; set; }

		public virtual bool Validate()
		{
			Errors.Clear();
			var errors = ValidationRules.Where(v => !v.Check(Value)).Select(v => v.ValidationMessage);
			Errors = errors.ToList();
			ErrorMsg = Errors.FirstOrDefault();
			IsValid = !Errors.Any();
			return IsValid;
		}

		public override string ToString()
		{
			return $"{Value}";
		}
	}
}