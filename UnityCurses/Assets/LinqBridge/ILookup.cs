using System.Collections.Generic;

namespace Assets.LinqBridge
{
	public interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>
	{
		int Count { get; }
		bool Contains (TKey key);
		IEnumerable<TElement> this [TKey key] { get; }
	}

}