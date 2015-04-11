using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public partial class CSTextBox
{
	string GetLastTypeIfDeclaring()
	{
		int end = SelectionStart;
		if (SelectionStart < Text.Length)
			if (!IsAlpha(Text[SelectionStart]))
				 end--;
		
		if (end == Text.Length)
			end--;
		
		var breakers = new [] { ';', '=', '(',  ')', '{', '}' };
		var separators = new [] { '.', ',' };
		
		bool shouldBreak = false;
		bool foundSeparator = false;
		
		while (end > 0)
		{
			if (!IsAlpha(Text[end]))
			{
				if (breakers.Contains(Text[end]))
					return "";
				
				shouldBreak = true;
				if (!foundSeparator)
					foundSeparator = separators.Contains(Text[end]);
			}
			else
			{
				if (foundSeparator)
					shouldBreak = false;
				
				if (shouldBreak)
					break;
				
				foundSeparator = false;
			}
			
			end--;
		}
		
		if (end == 0)
			return "";
		
		while (end > 0)
		{
			if (IsAlpha(Text[end]))
				break;
			
			end--;
		}
		
		int start = end++;
		while (start > -1)
		{
			if (!IsAlpha(Text[start]))
				break;
			
			start--;
		}
		
		start++;
		
		return Text.Substring(start, end - start);
	}
	
	/// <summary>
	/// Finds a type by a variable name
	/// </summary>
	/// <param name="variableName">The variable whose type is going to be found</param>
	/// <returns>The found type</returns>
	public Type FindType(string variableName)
	{
		var idx = Text.IndexOf(variableName, StringComparison.InvariantCulture);
		
		// Seeking before it
		int lstEnd = idx - 1;
		
		while (lstEnd > 0)
		{
			if (IsAlphaOrNumeric(Text[lstEnd]))
				break;
			lstEnd--;
		}
		
		int lstStart = lstEnd++ - 1;
		
		while (lstStart > -1)
		{
			if (!IsAlphaOrNumeric(Text[lstStart]))
				break;
			lstStart--;
		}
		
		lstStart++;
		
		var type = TranslateGenericType(Text.Substring(lstStart, lstEnd - lstStart));
		
		if (type == "var") // Seeking after =
		{
			lstStart = idx + variableName.Length;
			
			while (lstStart < Text.Length)
			{
				if (Text[lstStart] == '(') // explicit var
				{
					lstEnd = lstStart;
					
					while (lstEnd < Text.Length)
						if (Text[lstEnd++] == ')')
						{
							lstStart++;
							lstEnd--;
							break;
						}
					
					break;
				}
				else if (Text[lstStart] == 'n')
				{
					if (Text.Length < lstStart + 3)
						return null; // Nothing after "new"
					
					if (Text.Substring(lstStart, 3) == "new") // Seek for it
					{
						lstStart += 3;
						
						while (lstStart < Text.Length)
						{
							if (IsAlpha(Text[lstStart]))
								break;
							
							lstStart++;
						}
						
						lstEnd = lstStart;
						
						while (lstEnd < Text.Length)
						{
							if (Text[lstEnd] == '(')
								break;
							
							lstEnd++;
						}
						
						break;
					}
				}
				// else, it's from a method
				
				lstStart++;
			}
			
			type = Text.Substring(lstStart, lstEnd - lstStart);
		}
		
		return LoadedTypes.FirstOrDefault(t => t.Name == type);
	}
	
	string TranslateGenericType(string type)
	{
        switch (type)
        {
            case "string": return "String";
            case "int": return "Int32";
            case "decimal": return "Decimal";
            case "object": return "Object";
            case "void": return "Void";
            
            default: return type;
        }
	}
	
	/// <summary>
	/// Returns all the declared variables, except the paramethers from the methods
	/// </summary>
	/// <returns>The declared variables</returns>
	IEnumerable<string> GetDeclaredVariablesAndMethods()
	{
		var ms = VariablesRegex.Matches(GetValidTextToSearchTypesIn());
		
		foreach (Match m in ms)
		{
			bool first = true;
			foreach (Group g in m.Groups)
			{
				if (first)
				{
					first = false;
					continue;
				}
				
				if (g.Success)
					foreach (Capture c in g.Captures)
					{
						if (c.Index + c.Length == SelectionStart)
							continue;
						
						else
							yield return c.Value;
					}
			}
		}
	}
	
	string GetValidTextToSearchTypesIn()
	{
		return Text; // TODO [GetValidTextToSearchTypesIn] Create this method
	}
}
