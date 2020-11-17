using System.Text.RegularExpressions;
using CoreServices.ValidationService.Interfaces;

namespace CoreServices.ValidationService.Rules
{
	/// <summary>
	///     Make Sure that the object of type T is in the correct Email format
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class IsValidEmailRule<T> : IValidationRule<T>
	{
		public Regex EmailRegex { get; set; } =
			new Regex(
				@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
				RegexOptions.Compiled);

		public string ValidationMessage { get; set; }

		public bool Check(T value)
		{
			return EmailRegex.IsMatch($"{value}");
		}
	}
}