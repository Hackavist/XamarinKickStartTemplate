using System.Text.RegularExpressions;
using TemplateFoundation.Validation.Interfaces;

namespace TemplateFoundation.Validation.Rules
{
    public class IsNumbersOnlyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public Regex NumberRegex { get; set; } = new Regex(@"^\d+$", RegexOptions.Compiled);
        public bool Check(T value)
        {
            return NumberRegex.IsMatch($"{value}");
        }
    }
}
