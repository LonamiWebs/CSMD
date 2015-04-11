using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public partial class CSTextBox
{
	#region Is in method
	
	enum IIFTask { SeekingDot, SeekingEndType, SeekingStartMethod }
	
	bool IsInMethod()
	{
		var str = Text.Substring(0, SelectionStart);
		
		var opened = str.LastIndexOf('(');
		if (opened < 0)
			return false;
		
		var closed = Text.IndexOf(')', opened);
		
		int endMethod = closed < 0 || (closed > opened && SelectionStart < closed) ? opened : -1;
		
		return endMethod >= 0;
	}
	
	// TODO [GetCurrentMethod] Note that '(' will be valid, and "(" too... Plus, in comments too.
	IEnumerable<MethodInfo> GetCurrentMethod()
	{
		var str = Text.Substring(0, SelectionStart);
		// ( ) positions
		var opened = str.LastIndexOf('(');
		if (opened < 0)
			return null;
		
		var closed = Text.IndexOf(')', opened);
		
		int endMethod = closed < 0 || (closed > opened && SelectionStart < closed) ? opened : -1;
		
		if (endMethod < 0)
			return null;
		
		
		while (endMethod > -1)
		{
			if (IsAlpha(Text[endMethod]))
				break;
			endMethod--;
		}
		
		int startMethod = endMethod++;
		while (startMethod > -1)
		{
			if (!IsAlpha(Text[startMethod]))
				break;
			startMethod--;
		}
		startMethod++;
		
		// Getting type and method name
		string method = Text.Substring(startMethod, endMethod - startMethod);
		string type = GetLastWord(1, startMethod, true);
		
		if (method.Length == 0 || type.Length == 0)
			return null; // TODO [GetCurrentMethod] if (type.Length == 0) -> it is a made-by-user method
		
		var typ = LoadedTypes.FirstOrDefault(t => t.Name == type);
		
		if (typ == null || Instructions.Contains(method))
			return null;
		
		return typ.GetMethods().Where(m => m.Name == method);
	}
	
	#endregion
	
	int Count(string haystack, char needle)
	{
		int idx = 0;
		int count = 0;
		
		while (true)
		{
			idx = haystack.IndexOf(needle, idx);
			if (idx++ < 0)
				break;	
			count++;
		}
		
		return count;
	}
}
