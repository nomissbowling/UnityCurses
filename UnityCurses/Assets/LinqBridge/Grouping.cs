using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Assets.LinqBridge
{
	internal class Grouping<TKey, TElement> : ReadOnlyCollection<TElement>, IGrouping<TKey, TElement>
	{
		internal IList<TElement> InnerList { get { return this.Items; } }

		public TKey Key { get; private set; }

		public Grouping (TKey key) : base (new List<TElement>())
		{
			Key = key;			
		}
	}
}
