using System;
using System.Collections.Generic;
using System.Linq;
using TemplateFoundation.Helpers.HelperModels;

namespace TemplateFoundation.Helpers.HelperServices
{
    public static class SearchService
    {
        public static int Lcs(string firstString, string secondString)
        {
            int string1Length = firstString.Length;
            int string2Length = secondString.Length;
            var table = new int[string1Length + 1, string2Length + 1];
            for (int i = 0; i <= string1Length; i++)
            {
                for (int j = 0; j <= string2Length; j++)
                {
                    if (i == 0 || j == 0)
                        table[i, j] = 0;
                    else if (firstString[i - 1] == secondString[j - 1])
                        table[i, j] = table[i - 1, j - 1] + 1;
                    else
                        table[i, j] = Math.Max(table[i - 1, j], table[i, j - 1]);
                }
            }
            return table[string1Length, string2Length];
        }

        /// <summary>
        ///     Returns objects in the collection where its target property is identical or similar to the query string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="targetProperty"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IEnumerable<T> SearchInCollection<T>(IEnumerable<T> collection, string targetProperty, string query)
        {
            List<KeyVal<T, int>> searchList = collection.Select(x => new KeyVal<T, int>(x, 0)).ToList();
            foreach (var pair in searchList)
            {
                string stringValue = (string)typeof(T).GetProperty(targetProperty)?.GetValue(pair.Key, null);
                pair.Value = Lcs(stringValue, query);
            }

            return searchList.Where(x => x.Value > 0).OrderByDescending(x => x.Value).Select(x => x.Key);
        }

        /// <summary>
        ///     Returns objects in the collection where its target property is identical or similar to the query string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="targetPropertyFunc"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IEnumerable<T> SearchInCollection<T>(IEnumerable<T> collection, Func<T, string> targetPropertyFunc,
            string query)
        {
            List<KeyVal<T, int>> searchList = collection.Select(x => new KeyVal<T, int>(x, 0)).ToList();
            foreach (var pair in searchList)
            {
                string stringValue =
                    (string)typeof(T).GetProperty(targetPropertyFunc(pair.Key))?.GetValue(pair.Key, null);
                pair.Value = Lcs(stringValue, query);
            }

            return searchList.Where(x => x.Value > 0).OrderByDescending(x => x.Value).Select(x => x.Key);
        }
    }
}