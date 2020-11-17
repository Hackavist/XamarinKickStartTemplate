namespace CoreServices.ValidationService.Interfaces
{
	public interface IValidationRule<T>
	{
		string ValidationMessage { get; set; }
		bool Check(T value);
	}
}