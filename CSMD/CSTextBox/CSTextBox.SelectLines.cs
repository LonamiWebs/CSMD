
using System;
using System.Collections.Generic;

public partial class CSTextBox
{
	#region Lines selection
	
	#region Select lines
	
	/// <summary>
	/// Selects n lines starting by the first specified
	/// </summary>
	/// <param name="first">The first line specified</param>
	/// <param name="count">The count of lines to select</param>
	void SelectLines(int first, int count)
	{
		var lines = Text.Split('\n');
		
		int start = 0;
		for (int i = 0; i < first; i++)
			start += lines[i].Length + 1; // 1 = "\n".Length
		
		start++;

		int end = start;
		for (int i = 0; i < count; i++)
			end += lines[first + i].Length + 1; // 1 = "\n".Length
		
		end++;
		
		_SelectLinesBySSandSE(start, end);
	}
	
	/// <summary>
	/// Totally selects the lines occupied by the current selection
	/// </summary>
	void SelectLines()
	{ _SelectLinesBySSandSE(SelectionStart, SelectionStart + SelectionLength); }
	
	/// <summary>
	/// Select the lines occupied from the selection start to the selection end
	/// </summary>
	/// <param name="selectionStart">Where the selection starts</param>
	/// <param name="selectionEnd">Where the selection ends</param>
	void _SelectLinesBySSandSE(int selectionStart, int selectionEnd)
	{
		var indicies = GetNewLines();
		
		bool skip = false;
		
		int lst, start, end;
		lst = start = end = -1;
		
		foreach (var i in indicies)
		{
			skip = i < selectionStart;
			if (skip)
			{
				lst = i;
				continue;
			}
			
			if (start < 0)
				start = lst;
			
			skip = i < selectionEnd;
			if (skip)
				continue;
			
			end = i;
			break;
		}
		
		start++;
		
		SelectionStart = start;
		
		int l = end - start;
		if (l > -1)
			SelectionLength = l;
	}
	
	#endregion
	
	#region Retrieve selected lines
	
	/// <summary>
	/// Retrieves the first selected line index and how much lines are selected 
	/// </summary>
	/// <returns>[0] = first line; [1] = line count</returns>
	int[] GetFirstAndCountOfSelectedLines()
	{	
		var indicies = GetNewLines();
			
		bool skip = false;
		
		int start = -1, count = 0;
		
		int selectionEnd = SelectionStart + SelectionLength;
		
		foreach (var i in indicies)
		{
			skip = i < SelectionStart;
			if (skip)
			{
				start++;
				continue;
			}
			
			skip = i < selectionEnd;
			if (skip)
			{
				count++;
				continue;
			}
			
			break;
		}
		
		return new [] { start, count };
	}
	
	#endregion
	
	#region Utils for counting
	
	/// <summary>
	/// Retrieves the indicies of where the lines start (by character index)
	/// </summary>
	/// <returns>The indicies</returns>
	IEnumerable<int> GetNewLines()
	{
		yield return 0;
		
		int i = 0;
		
		i = Text.IndexOf('\n', i);
		
		while (i >= 0)
		{
			yield return i;
			
			i++;
			i = Text.IndexOf('\n', i);
		}
		
		yield return Text.Length;
	}
	
	#endregion
	
	#endregion
}
