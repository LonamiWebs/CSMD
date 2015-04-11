using System;
using System.Linq;

public partial class CSTextBox
{
	#region Words
	
	/// <summary>
	/// Gets the last word start index and end index
	/// </summary>
	/// <returns>[0] = StartIndex; [1] = EndIndex</returns>
	int[] GetCurrentWordStartAndEnd()
	{
		int end = SelectionStart;
		while (end < Text.Length)
		{
			if (!IsAlpha(Text[end]))
				break;
			
			end++;
		}
		
		int start = SelectionStart;
		if (SelectionStart < Text.Length)
			if (!IsAlpha(Text[SelectionStart]))
				 start--;
		
		if (start == Text.Length)
			start--;
		
		while (start > 0)
		{
			if (!IsAlpha(Text[start]))
			{
				start++;
				break;
			}
			start--;
		}
		
		return new [] { start, end };
	}
	
	/// <summary>
	/// Retrieves the last word
	/// </summary>
	/// <param name="deep">The search deepness. 0 = current; 1 = last; 2 before last; etc</param>
	/// <param name="seekForDot">Should the words be separated only by a dot?</param>
	/// <returns>The last word</returns>
	string GetLastWord(int deep = 1, bool seekForDot = false)
	{ return GetLastWord(deep, SelectionStart, seekForDot); }
	
	/// <summary>
	/// Retrieves the last word
	/// </summary>
	/// <param name="deep">The search deepness. 0 = current; 1 = last; 2 before last; etc</param>
	/// <param name="seekFrom">Where should it start seeking from?</param>
	/// <param name="seekForDot">Should the words be separated only by a dot?</param>
	/// <returns>The last word</returns>
	string GetLastWord(int deep, int seekFrom, bool seekForDot)
	{
		if (deep == 0)
		{
			int end = seekFrom;
			while (end < Text.Length)
			{
				if (!IsAlpha(Text[end]))
					break;
				
				end++;
			}
			
			int start = seekFrom;
			if (seekFrom < Text.Length)
				if (!IsAlpha(Text[seekFrom]))
					 start--;
			
			if (start == Text.Length)
				start--;
			
			while (start > 0)
			{
				if (!IsAlpha(Text[start]))
				{
					start++;
					break;
				}
				start--;
			}
			
			if (start < 0)
				start++;
			
			return Text.Substring(start, end - start);
		}
		else
		{	
			int end = seekFrom;
			if (seekFrom < Text.Length)
				if (!IsAlpha(Text[seekFrom]))
					 end--;
			
			if (end == Text.Length)
				end--;
			
			bool found = false;
			
			while (deep > 0)
			{
				while (end > 0)
				{
					if (!IsAlpha(Text[end]))
					{
						FoundParenthesis = SeekForParenthesis(end);
						break;
					}
					
					end--;
				}
				
				if (end == 0)
					return "";
				
				while (end > 0)
				{
					if (IsAlpha(Text[end]))
						break;
					
					else if (seekForDot && Text[end] == '.')
					{
						if (found)
							return "";
						found = true;
					}
					
					end--;
				}
				
				if (seekForDot && !found)
					return "";
				
				deep--;
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
	}
	
	
	bool SeekForParenthesis(int position)
	{
		position--;
		
		while (position > -1)
		{
			if (Text[position] == ')')
				return true;
			
			if (!char.IsWhiteSpace(Text[position--]))
				return false;
		}
		
		return false;
	}
	
	#endregion
}