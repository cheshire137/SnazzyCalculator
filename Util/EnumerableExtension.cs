using System;
using System.Collections.Generic;

namespace Util
{
	public static class EnumerableExtension
	{
		/* Thanks to http://blogs.msdn.com/b/ericwhite/archive/2010/02/15/rollup-extension-method-create-running-totals-using-linq-to-objects.aspx */
		public static IEnumerable<TResult> Rollup<TSource, TResult>(
			this IEnumerable<TSource> source, TResult seed,
			Func<TSource, TResult, TResult> projection
		)
		{
			TResult nextSeed = seed;
			foreach (TSource src in source)
			{
				TResult projectedValue = projection(src, nextSeed);
				nextSeed = projectedValue;
				yield return projectedValue;
			}
		}
		
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
		
		/* Thanks to http://blogs.msdn.com/b/ericwhite/archive/2009/07/05/comparing-two-open-xml-documents-using-the-zip-extension-method.aspx */
		public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(
			this IEnumerable<TFirst> first, IEnumerable<TSecond> second,
			Func<TFirst, TSecond, TResult> func)
		{
			var ie1 = first.GetEnumerator();
			var ie2 = second.GetEnumerator();
			while (ie1.MoveNext() && ie2.MoveNext())
			{
				yield return func(ie1.Current, ie2.Current);
			}
		}
	}
}

