using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Util
{
	public static class StringExtension
	{
		public static string StringConcatenate(this IEnumerable<string> source)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string s in source)
			{
				sb.Append(s);
			}
			return sb.ToString();
		}
 
		public static string StringConcatenate<T>(
            this IEnumerable<T> source,
            Func<T, string> projectionFunc)
		{
			return source.Aggregate(new StringBuilder(),
                (s, i) => s.Append(projectionFunc(i)),
                s => s.ToString());
		}
	}
}

