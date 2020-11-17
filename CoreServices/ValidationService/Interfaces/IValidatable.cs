using System.Collections.Generic;
using System.ComponentModel;

namespace CoreServices.ValidationService.Interfaces
{
	public interface IValidatable<T> : INotifyPropertyChanged
	{
		List<IValidationRule<T>> ValidationRules { get; }

		List<string> Errors { get; set; }

		bool IsValid { get; set; }

		bool Validate();
	}
}