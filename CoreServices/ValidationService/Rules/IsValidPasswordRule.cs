using System.Text.RegularExpressions;
using CoreServices.ValidationService.Interfaces;

namespace CoreServices.ValidationService.Rules
{
	/// <summary>
	///     Makes sure that a password is between 8-20 digits, one upper case character, one lower case character, one number,
	///     and one special character.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class IsValidPasswordRule<T> : IValidationRule<T>
	{
		public Regex RegexPassword { get; set; } =
			new Regex("(?=.*[A-Z])(?=.*\\d)(?=.*[¡!@#$%*¿?\\-_.\\(\\)])[A-Za-z\\d¡!@#$%*¿?\\-\\(\\)&]{8,20}");

		public string ValidationMessage { get; set; }

		public bool Check(T value)
		{
			return RegexPassword.IsMatch($"{value}");
		}
	}
}