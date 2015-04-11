using System;

public partial class CSTextBox
{
	#region Text
	
	/// <summary>
	/// Replaces the last word by the specified one
	/// </summary>
	/// <param name="text"></param>
	void ReplaceLastWord(string text)
	{
		if (text.Length == 0)
			return;
		
		var sae = GetCurrentWordStartAndEnd();
		
		if (sae[0] < 0)
			return;
		    
		Text = Text.Substring(0, sae[0]) + text + Text.Substring(sae[1], Text.Length - sae[1]);
		
		SelectionStart = sae[0] + text.Length;
	}
	
	/// <summary>
	/// Appends the specified text and moves the selection start to the end of the appended text
	/// </summary>
	/// <param name="text">The text to append</param>
	new void AppendText(string text) { AppendText(text, text.Length); }
	
	/// <summary>
	/// Appends the specified text and moves the selection start n positions
	/// </summary>
	/// <param name="text">The text to append</param>
	/// <param name="move">The positions to move</param>
	void AppendText(string text, int move)
	{	
		int old = SelectionStart;
		
		Text = Text.Substring(0, old) + text + Text.Substring(old, Text.Length - old);
		
		SelectionStart = old + move;
	}
	
	/// <summary>
	/// Appends a new line
	/// </summary>
	void NewLine()
	{
		bool irabfnli = IsRightAfterBracketForNewLineIndenting();
		
		if (SelectionStart < Text.Length && Text[SelectionStart] == '}' && irabfnli)
		{
			if (irabfnli)
			{
				var tabs = new string('\t', LineTabs + 1);
				var tabl = new string('\t', LineTabs);
				
				AppendText(Environment.NewLine + tabs + Environment.NewLine + tabl, tabs.Length + 1);
			}
			else
			{
				var tabs = new string('\t', LineTabs);
				AppendText(Environment.NewLine + tabs, tabs.Length + 1);
			}
		}
		else
		{
			var tabs = new string('\t', irabfnli ? LineTabs + 1 : LineTabs);
			AppendText(Environment.NewLine + tabs, tabs.Length + 1);
		}
	}
	
	bool IsRightAfterBracketForNewLineIndenting()
	{
		int i = SelectionStart - 1;
		while (i > -1)
		{
			if (Text[i] == '{')
				return true;
			
			if (!char.IsWhiteSpace(Text[i]) || Text[i] == '\n')
				return false;
			
			i--;
		}
		
		return false;
	}

	/// <summary>
	/// Checks whether a character is or not alpha
	/// </summary>
	/// <param name="c">The character to check</param>
	/// <returns>True if it is alpha</returns>
	public static bool IsAlpha(char c)
	{
		int val = (int)c;
		return (val >= LowerCaseStart && val <= LowerCaseEnd)
			|| (val >= UpperCaseStart && val <= UpperCaseEnd)
			|| (val >= AccentsStart && val <= AccentsEnd);
	}

	/// <summary>
	/// Checks whether a character is or not alpha (or numeric)
	/// </summary>
	/// <param name="c">The character to check</param>
	/// <returns>True if it is alpha</returns>
	public static bool IsAlphaOrNumeric(char c)
	{
		int val = (int)c;
		return (val >= LowerCaseStart && val <= LowerCaseEnd)
			|| (val >= UpperCaseStart && val <= UpperCaseEnd)
			|| (val >= AccentsStart && val <= AccentsEnd)
			|| (val >= NumericStart && val <= NumericEnd);
	}
	
	#endregion
}