using System;
using System.Collections.Generic;

namespace GA.Collections
{
    public static class CollectionExtensions
    {
        public static void Swap<T>(this IList<T> target, int indexA, int indexB)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            T temp = target[indexA];
            target[indexA] = target[indexB];
            target[indexB] = temp;
        }
    }
}