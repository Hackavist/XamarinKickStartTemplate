using System.Text;

namespace TemplateFoundation.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string FormatFromCamelCase(this string s)
        {
            if (s.Trim().Contains(" ")) return s;
            StringBuilder builder = new StringBuilder(s);
            for (int i = 1; i < builder.Length; i++)
            {
                if (char.IsUpper(builder[i]))
                {
                    builder.Insert(i, ' ');
                    i++;
                }
            }

            return builder.ToString();
        }

        public static string FormatAndPluralize(this string s)
        {
            s = FormatFromCamelCase(s).Trim();
            if (s.EndsWith('y'))
                s = s.Remove(s.Length - 1) + "ies";
            else
                s += "s";
            return s;
        }
    }
}