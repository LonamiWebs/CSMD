using System;
using System.Collections.Generic;
using System.Text;

public partial class CSTextBox
{
	List<string> GetUsings()
	{
		string[] usings = Text.Split(new [] { "using " }, StringSplitOptions.None);
		var validUsings = new List<string>();
		foreach (var @using in usings)
		{
			string u = @using.Split(';')[0].Replace(" ", "");
			if (u.Equals(String.Empty))
				continue;
			
			foreach (var c in u)
				if (!IsAlpha(c))
					continue;
			
			if (Text.Replace(" ", "").Contains("using" + u + ";"))
				validUsings.Add(u);
		}
		return validUsings;
	}
	
	void AddUsing(string @using)
	{
		var usings = GetUsings();
		if (usings.Contains(@using))
			return;
		
		usings.Add(@using);
		UpdateUsings(usings);
	}
	
	void UpdateUsings(List<string> usings)
	{
		var lines = new List<string>(Lines);
		int removed = 0;
		for (int i = 0; i < lines.Count; i++)
		{
			if (lines[i].Contains("namespace"))
				break;
			
			if (lines[i].Contains("using ")) {
				removed += lines[i].Length + 2; // 2 = "\r\n".Length
				lines.RemoveAt(i--);
			}
		}
		
		var sb = new StringBuilder();
		foreach (var u in usings)
			sb.AppendLine("using " + u + ";");
		
		int added = sb.ToString().Length;
		
		string newstr = sb + String.Join("\r\n", lines);
		
		int pos = SelectionStart += added - removed;
		Text = newstr;
		SelectionStart = pos;
	}
}
