using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using ExtensionMethods;

public partial class CSTextBox
{
	#region Autocomplete
	
	#region Show and hide
	
	/// <summary>
	/// Shows the AutocompleteForm
	/// </summary>
	void ShowAutocomplete(IEnumerable<MethodInfo> mi = null)
	{
		Point pt = PointToScreen(GetPositionFromCharIndex
        	(SelectionStart > 0 ? SelectionStart - 1 : 0));
		
		if (!Autocomplete.Suggesting)
			Autocomplete.Show(pt.X + AutocompleteOffset.X, pt.Y + AutocompleteOffset.Y);
		
		else
			Autocomplete.Location = new Point(pt.X + AutocompleteOffset.X, pt.Y + AutocompleteOffset.Y);
		
		Autocomplete.ToggleMode(mi);
	}
	
	#endregion
	
	#region Suggestions
	
	/// <summary>
	/// Refresh the suggestions in the autocomplete
	/// </summary>
	void RefreshSuggestions()
	{
		if (LoadingAssemblies)
			return;
		
		ShowAutocomplete();
		
		Autocomplete.BeginUpdate();
		
		var suggestions = new List<string>();
		string lstType = GetLastWord(1, true);
		string curWord = GetLastWord(0);
		string lstWord = GetLastWord();
		
		int added = 0;
		
		// using Namespace
		if (lstWord == "using")
		{
			foreach (var ns in LoadedNamespaces)
			{
				if (!ns.Contains(curWord, StringComparison.InvariantCultureIgnoreCase))
					continue;
				
				if (added++ >= MaximumSuggestions)
					break;
				
				Autocomplete.AddSuggestion(new Suggestion(ns, "namespace", ""));
			}
		}
		// Type.Something
		else if (lstType.Length > 0)
		{
			var type = LoadedTypes.FirstOrDefault(t => t.Name == lstType);
			bool mustStatic = !FoundParenthesis;
			
			if (type == null)
			{
				type = FindType(lstType);
				mustStatic = false;
			}
			
			if (type != null)
			{
				if (type.IsEnum)
				{
					foreach (var f in type.GetFields())
					{	
						if (!f.Name.Contains(curWord, StringComparison.InvariantCultureIgnoreCase))
							continue;
						
						if (added++ >= MaximumSuggestions)
							break;
						
						if (f.Name != "value__")
							Autocomplete.AddSuggestion(new Suggestion(f.Name, "enumprop", ""));
					}
				}
				else
				{
					foreach (var m in type.GetMethods())
					{
						if (m.IsStatic != mustStatic || !m.IsPublic ||
						    !m.Name.Contains(curWord, StringComparison.InvariantCultureIgnoreCase))
							continue;
						
						if (added++ >= MaximumSuggestions)
							break;
						
						if (m.Name.IndexOf("get_", StringComparison.InvariantCulture) == 0 ||
						    m.Name.IndexOf("set_", StringComparison.InvariantCulture) == 0)
							Autocomplete.AddSuggestion(new Suggestion(m.Name.Substring(4), "property", ""));
						
						else if (m.Name.IndexOf("add_", StringComparison.InvariantCulture) == 0)
							Autocomplete.AddSuggestion(new Suggestion(m.Name.Substring(4), "event", ""));
						
						else if (m.Name.IndexOf("remove_", StringComparison.InvariantCulture) == 0)
							Autocomplete.AddSuggestion(new Suggestion(m.Name.Substring(7), "event", ""));
						
						else
							Autocomplete.AddSuggestion(new Suggestion(m.Name, "method", ""));
					}
				}
			}
		}
		// No type has been declared
		else
		{
			foreach (var v in GetDeclaredVariablesAndMethods())
			{
				if (!v.Contains(curWord, StringComparison.InvariantCultureIgnoreCase))
					continue;
				
				if (added++ >= MaximumSuggestions)
					break;
				
				Autocomplete.AddSuggestion(new Suggestion(v, "custom", ""));
			}
			foreach (var k in Keywords)
			{
				if (!k.Contains(curWord, StringComparison.InvariantCultureIgnoreCase))
					continue;
				
				if (added++ >= MaximumSuggestions)
					break;
				
				Autocomplete.AddSuggestion(new Suggestion(k, "keyword", ""));
			}
			foreach (var t in LoadedTypes)
			{	
				if (!t.IsPublic || !t.Name.Contains(curWord, StringComparison.InvariantCultureIgnoreCase)
				    || t.Name.Contains("`")) // Well, about this... Maybe the user wants.
					continue;
				
				if (added++ >= MaximumSuggestions)
					break;
				
				if (t.IsEnum)
					Autocomplete.AddSuggestion(new Suggestion(t.Name, "enum", t.Namespace));
				else if (t.IsClass)
					Autocomplete.AddSuggestion(new Suggestion(t.Name, "class", t.Namespace));
				else
					Autocomplete.AddSuggestion(new Suggestion(t.Name, "method", t.Namespace));
			}
		}
		
		Autocomplete.EndUpdate(curWord);
	}
	
	void RefreshSuggestions(IEnumerable<MethodInfo> mi)
	{
		if (mi == null)
			RefreshSuggestions();
		
		else
			ShowAutocomplete(mi);
	}
	
	#endregion
	
	#region Select suggestion
	
	/// <summary>
	/// Selects the current suggestion
	/// </summary>
	void SelectSuggestion()
	{
		var sug = Autocomplete.SelectSuggestion();
		if (sug == null)
			return;
		
		ReplaceLastWord(sug.Name);
		
		if (sug.Namespace.Length > 0)
			AddUsing(sug.Namespace);
	}
	
	#endregion
	
	#endregion
}