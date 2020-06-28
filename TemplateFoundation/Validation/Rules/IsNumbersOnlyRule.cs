using TemplateFoundation.Validation.Interfaces;

namespace TemplateFoundation.Validation.Rules
{
    public class IsNumbersOnlyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            var str = $"{value}";
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
    }
}
