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

        public static void Reverse<T>(this IList<T> list)
        {
            if(list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            for (int i = 0; i < list.Count / 2; i++)
            {
                int startIndex = i;
                int endIndex = list.Count - 1 - i;

                list.Swap(startIndex, endIndex);
            }
        }
    }
}