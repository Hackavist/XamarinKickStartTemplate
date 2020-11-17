using System.Text.RegularExpressions;
using CoreServices.ValidationService.Interfaces;

namespace CoreServices.ValidationService.Rules
{
	public class IsNumbersOnlyRule<T> : IValidationRule<T>
	{
		public Regex NumberRegex { get; set; } = new Regex(@"^\d+$", RegexOptions.Compiled);
		public string ValidationMessage { get; set; }

		public bool Check(T value)
		{
			return NumberRegex.IsMatch($"{value}");
		}
	}
}