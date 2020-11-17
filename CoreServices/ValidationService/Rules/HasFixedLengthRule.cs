using CoreServices.ValidationService.Interfaces;

namespace CoreServices.ValidationService.Rules
{
	public class HasFixedLengthRule<T> : IValidationRule<T>
	{
		public int Length { get; set; }

		public HasFixedLengthRule(int length)
		{
			Length = length;
		}

		public string ValidationMessage { get; set; }

		public bool Check(T value)
		{
			string val = $"{value}";
			return val.Length == Length;
		}
	}
}