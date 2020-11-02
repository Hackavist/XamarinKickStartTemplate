using System.Text.RegularExpressions;
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
        public Regex EmailRegex { get; set; } = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
        public bool Check(T value)
        {
            return EmailRegex.IsMatch($"{value}");
        }
    }
}
