
using System;

public partial class CSTextBox
{	
	#region Indent
	
	/// <summary>
	/// Indents the current selection in n tabs
	/// </summary>
	/// <param name="count">Tab count to indent</param>
	void Indent(int count = 1)
	{
		var lines = Text.Split('\n');
		var fal = GetFirstAndCountOfSelectedLines();
		
		if (count > 0)
		{
			for (int i = 0; i <= fal[1]; i++)
				lines[fal[0] + i] =  new string('\t', count) + lines[fal[0] + i];
		}
		else if (count < 0)
		{
			for (int i = 0; i <= fal[1]; i++)
			{
				if (lines[fal[0] + i].StartsWith("\t", StringComparison.InvariantCulture))
					lines[fal[0] + i] = lines[fal[0] + i].Substring(1);
				
				else if (lines[fal[0] + i].StartsWith(new string(' ', SpacesAsTab), StringComparison.InvariantCulture))
					lines[fal[0] + i] = lines[fal[0] + i].Substring(SpacesAsTab);
			}
		}
		
		Text = String.Join("\n", lines);
		
		SelectLines(fal[0], fal[1]);
	}
	
	#endregion
}