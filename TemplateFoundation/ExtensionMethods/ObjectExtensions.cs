using System.Reflection;

namespace TemplateFoundation.ExtensionMethods
{
    public static class ObjectExtensions
    {
        public static bool HasNullOrEmptyStrings<T>(this T obj)
        {
            if (obj == null) return true;
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (!propertyInfo.CanRead) continue;
                if (propertyInfo.PropertyType != typeof(string)) continue;
                string val = (string)propertyInfo.GetValue(obj);
                return string.IsNullOrWhiteSpace(val);
            }

            return false;
        }
    }
}