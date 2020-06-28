using TemplateFoundation.Validation.Interfaces;

namespace TemplateFoundation.Validation.Rules
{
    /// <summary>
    /// Make Sure that the object of type T is in the correct Email format
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IsValidEmailRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress($"{value}");
                return addr.Address == $"{value}";
            }
            catch
            {
                return false;
            }
        }
    }
}
