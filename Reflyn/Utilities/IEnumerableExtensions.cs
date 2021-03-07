using System.Collections.Generic;

namespace Reflyn.Utilities
{
    public static class IEnumerableExtensions
    {
        // https://stackoverflow.com/questions/753316/extension-method-for-enumerable-intersperse
        public static IEnumerable<T> Intersperse<T>(this IEnumerable<T> source, T element)
        {
            bool first = true;
            foreach (T value in source)
            {
                if (!first) yield return element;
                yield return value;
                first = false;
            }
        }
    }
}
