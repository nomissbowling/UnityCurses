using System.Collections.Generic;

namespace Assets.LinqBridge.Query_Operators
{
	public static partial class Enumerable
	{
		// OrderBy:

		public static System.Linq.IOrderedEnumerable<TSource> OrderBy<TSource, TKey> (
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector)
		{
			return OrderBy (source, keySelector, null);
		}

		public static System.Linq.IOrderedEnumerable<TSource> OrderBy<TSource, TKey> (
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector, 
			IComparer<TKey> comparer)
		{
			return new OrderByEnumerable<TSource, TKey> (source, keySelector, null, false);
		}

		public static System.Linq.IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey> (
			this IEnumerable<TSource> source, 
			Func<TSource, TKey> keySelector)
		{
			return OrderByDescending (source, keySelector, null);
		}

		public static System.Linq.IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey> (
			this IEnumerable<TSource> source, 
			Func<TSource, TKey> keySelector,
			IComparer<TKey> comparer)
		{
			return new OrderByEnumerable<TSource, TKey> (source, keySelector, null, true);
		}

		// ThenBy:

		public static System.Linq.IOrderedEnumerable<TSource> ThenBy<TSource, TKey> (
			this System.Linq.IOrderedEnumerable<TSource> source,
			Func<TSource, TKey> keySelector)
		{
			return ThenBy (source, keySelector, null);
		}

		public static System.Linq.IOrderedEnumerable<TSource> ThenBy<TSource, TKey> (
			this System.Linq.IOrderedEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			IComparer<TKey> comparer)
		{
			return source.CreateOrderedEnumerable<TKey> (keySelector, comparer, false);
		}

		public static System.Linq.IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey> (
			this System.Linq.IOrderedEnumerable<TSource> source, 
			Func<TSource, TKey> keySelector)
		{
			return ThenByDescending (source, keySelector, null);
		}

		public static System.Linq.IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey> (
			this System.Linq.IOrderedEnumerable<TSource> source,
			Func<TSource, TKey> keySelector, 
			IComparer<TKey> comparer)
		{
			return source.CreateOrderedEnumerable<TKey> (keySelector, comparer, true);
		}
	}
}
