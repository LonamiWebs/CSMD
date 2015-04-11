using System;

namespace ExtensionMethods
{
	public static class Strings
	{
		public static bool Contains(this string haystack, string needle, StringComparison comparision)
		{ return haystack.IndexOf(needle, comparision) >= 0; }
	}
}
