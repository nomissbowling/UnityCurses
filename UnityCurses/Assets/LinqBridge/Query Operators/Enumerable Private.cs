using System;

namespace Assets.LinqBridge.Query_Operators
{
	public static partial class Enumerable
	{
		static void ThrowNoElements ()
		{
			throw new InvalidOperationException ("Enumerable contains no elements");
		}

		static void ThrowNoMatches ()
		{
			throw new InvalidOperationException ("Enumerable contains no matching element");
		}
	}
}
