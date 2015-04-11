using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

public partial class CSTextBox
{
	#region Overrides
	
	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (!AutocompletionEnabled)
			return;
		
		MustRefresh = false;
		
		switch (e.KeyCode)
		{
			case Keys.Enter:
				
				if (Autocomplete.Suggesting)
					SelectSuggestion();
				
				else
					NewLine();
				
				e.Handled = e.SuppressKeyPress = true;
				break;
				
			case Keys.Tab:
				
				if (Autocomplete.Suggesting)
				{
					SelectSuggestion();
					e.Handled = e.SuppressKeyPress = true;
				}
				else
				{
					if (SelectionLength > 0 || e.Shift)
					{
						Indent(e.Shift ? -1 : 1);
						e.Handled = e.SuppressKeyPress = true;
					}
					
				}
				
				break;
				
			case Keys.Home:
				
				Autocomplete.Hide();
				
				// TODO EmulateHome(e.Shift);
				
				// e.Handled = e.SuppressKeyPress = true;
				
				break;
				
			case Keys.End:
			case Keys.Right:
			case Keys.Left:
				
				Autocomplete.Hide();
				
				break;
				
			case Keys.Up:
				
				if (Autocomplete.Visible)
				{
					Autocomplete.MoveUp();
					e.Handled = e.SuppressKeyPress = true;
				}
				
				break;
				
			case Keys.Down:
				
				if (Autocomplete.Visible)
				{
					Autocomplete.MoveDown();
					e.Handled = e.SuppressKeyPress = true;
				}
				
				break;
				
			case Keys.Space:
				
				if (Autocomplete.Suggesting)
					SelectSuggestion();
				break;
				
			case Keys.OemPeriod:
				
				if (Autocomplete.Suggesting)
				{
					SelectSuggestion();
					MustRefresh = true;
				}
				
				break;
				
			case Keys.Delete:
			case Keys.Back:
				
				int c = SelectionStart - 2;
				
				if (c >= 0 && IsAlpha(Text[c]))
					MustRefresh = true;
				else
					Autocomplete.Hide();
				
				break;
				
			case Keys.V:
				
				if (e.Control)
					if (String.IsNullOrEmpty(Clipboard.GetText()))
						Clipboard.Clear(); // bye bye images
				
				else
					MustRefresh = true;
				
				break;
				
			case Keys.ShiftKey:
			case Keys.Menu:
			case Keys.Control:
			case Keys.Alt:
				
				// do nothing
				
				break;
				
			default:
				var kc = (int)e.KeyCode;
				
				// TODO [OnKeyDown] Do Not Suggest In Comments
				if (e.Control || e.EqualsChar(')') || (InStrOrDeclaring = IsInString() || IsDeclaring()) && !IsInMethod())
					Autocomplete.Hide();
				
				else
				{
					MustRefresh = ValidKeyForName(e.KeyCode, e.Shift);
					
					if (Autocomplete.Suggesting && !MustRefresh)
						SelectSuggestion();
				}
				
				break;
				
			case Keys.Escape:
				
				GetValidTextToSearchTypesIn();
				
				Autocomplete.Hide();
				
				break;
		}
		
		base.OnKeyDown(e);
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		if (!AutocompletionEnabled)
			return;
		
		if (e.EqualsChar(')'))
			Autocomplete.Hide();
		
		else if (MustRefresh || e.EqualsChar('('))
		{
			var mis = GetCurrentMethod();
			
			if (mis != null)
				RefreshSuggestions(mis);
			
			else if (!InStrOrDeclaring)
				RefreshSuggestions();
			
			MustRefresh = false;
		}
		
		base.OnKeyUp(e);
	}
	
	protected override void OnClick(EventArgs e)
	{
		if (!AutocompletionEnabled)
			return;
		
		Autocomplete.Hide();
		base.OnClick(e);
	}
	
	protected override void OnLostFocus(EventArgs e)
	{
		if (!AutocompletionEnabled)
			return;
		
		if (!Autocomplete.ContainsMouse)
			Autocomplete.Hide();
		
		base.OnLostFocus(e);
	}
	
	#endregion
	
	#region Validators
	
	/// <summary>
	/// Checks if the pressed key is valid for a name
	/// </summary>
	/// <param name="key">The key to check</param>
	/// <param name="shift">Is shift being held down?</param>
	/// <returns>True if it's valid</returns>
	bool ValidKeyForName(Keys key, bool shift)
	{
		int k = (int)key;
		
		var lstWord = GetLastWord(0);
		
		if (lstWord.Length == 0) // Maybe it's only number - so a number for the first char of a name is not valid
			return	(k >= ACode && k <= ZCode) ||
					(shift && key == Keys.OemMinus) ||
					key == Keys.Oem1 || key == Keys.Oem7;
	
		return	(k >= ACode && k <= ZCode) ||
				(shift && key == Keys.OemMinus) ||
				(!shift && k >= Code0 && k <= Code9) ||
				key == Keys.Oem1 || key == Keys.Oem7;
	}
	
	#endregion
}