using System;
using System.Collections.Generic;

namespace Util
{
	public static class EnumerableExtension
	{
		public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source,
            int count)
        {
            Queue<T> saveList = new Queue<T>();
            int saved = 0;
            foreach (T item in source)
            {
                if (saved < count)
                {
                    saveList.Enqueue(item);
                    ++saved;
                    continue;
                }
                saveList.Enqueue(item);
                yield return saveList.Dequeue();
            }
            yield break;
        }
	}
}

