using System.Collections.Generic;

namespace Assets.LinqBridge
{
	public interface IGrouping<TKey, TElement> : IEnumerable<TElement>
	{
		TKey Key { get; }
	}
}
