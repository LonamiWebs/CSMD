using System;
using System.Collections.Generic;

public partial class CSTextBox
{
	#region Is in string
	
	/// <summary>
	/// Returns whether the selection start in a string or not
	/// </summary>
	/// <returns>True if it's in a string</returns>
	bool IsInString()
	{ return IsInString(SelectionStart); }
	
	/// <summary>
	/// Returns whether the specified position is in a string or not
	/// </summary>
	/// <param name="position">The specified position</param>
	/// <returns>True if it's in a string</returns>
	bool IsInString(int position)
	{
		string str = Text.Substring(0, position);
		
		bool inNormal = false, inLiteral = false;
		
		int lstIdx = 0;
		int idx = 0;
		
		do
		{
			idx = str.IndexOf('"', idx);
			
			if (idx < 0)
				return inNormal || inLiteral;
			
			if (inNormal)
			{
				if (idx < str.Length - 1 && (str[idx - 1] == '\'' && str[idx + 1] == '\''))
					inNormal = false;
					
				else
					inNormal = str[idx - 1] == '\\';
			}
			else if (inLiteral)
			{
				if (idx < str.Length - 1)
				{
					if (str[idx + 1] == '"')
						idx += 2;
					else
						inLiteral = false;
				}
				else
					inLiteral = false;
			}
			else
			{
				if (idx > 0)
				{
					if (str[idx - 1] == '@')
						inLiteral = true;
					else
						inNormal = true;
				}
				else
					inNormal = true;
			}
			
			if (position > lstIdx && position <= idx)
				return inNormal || inLiteral;
			
			lstIdx = idx++;
		}
		while (idx > 0);
			
		return false;
	}
	
	#endregion
}