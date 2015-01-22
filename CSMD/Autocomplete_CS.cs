﻿/* Made by Lonami Exo.
 * 
 * ===== Started =====
 * 20 - January - 2015
 * ==== Last edit ====
 * 22 - January - 2015
 * 
 * Me llevóh toah' lah
 * tarde pero funca */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

//todo tooltip which assembly is it (on mouse in)
public class Autocomplete {
	
	#region Private variables

	readonly TextBox textbox;
	readonly Form af;
	readonly ListBox lb;
	readonly ContextMenuStrip cms;

	List<Type> loadedTypes = new List<Type>();

	bool lb_doubleclick;

	const string rnonalpha = @"\W|_"; // match any non-word or _
	const string rnonalpha_nodot = @"[^\w.]"; // match any non-word and non-.

	bool is64bitPC = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));
	readonly string assembliesLocation;

	readonly List<string> keywords = new List<string> {
		"abstract", "as", "base", "bool", "break", "byte", "case", "catch",
		"char", "checked", "class", "const", "continue", "decimal", "default",
		"delegate", "do", "double", "else", "enum", "event", "explicit",
		"extern", "false", "finally", "fixed", "float", "for", "foreach",
		"goto", "if", "implicit", "in", "int", "interface", "internal", "is",
		"lock", "long", "namespace", "new", "null", "object", "operator",
		"out", "override", "params", "private", "protected", "public",
		"readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof",
		"stackalloc", "static", "string", "struct", "switch", "this",
		"throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
		"unsafe", "ushort", "using", "virtual", "void", "volatile", "while",
		"add", "alias", "ascending", "async", "await", "descending",
		"dynamic", "from", "get", "global", "group", "into", "join", "let",
		"orderby", "partial", "remove", "select", "set", "value", "var", "where", "yield"
	};
	
	List<string> usings = new List<string>();
	bool _AddUsingsOnTheFly;
	
	#endregion

	#region Extern, statics and const

	const int SW_SHOWNOACTIVATE = 4;
	const int HWND_TOPMOST = -1;
	const uint SWP_NOACTIVATE = 0x0010;

	[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
	static extern bool SetWindowPos(
	     int hWnd, // Window handle
	     int hWndInsertAfter, // Placement-order handle
	     int X, // Horizontal position
	     int Y, // Vertical position
	     int cx, // Width
	     int cy, // Height
	     uint uFlags); // Window positioning flags

	[DllImport("user32.dll")]
	static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

	static void ShowInactiveTopmost(Form frm)
	{
		ShowWindow(frm.Handle, SW_SHOWNOACTIVATE);
		SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST,
	     frm.Left, frm.Top, frm.Width, frm.Height,
	     SWP_NOACTIVATE);
	}

	#endregion

	#region Public variables and properties

	public Size AutocompleteSize {
		get { return af.Size; }
		set { af.Size = value; }
	}

	public Point AutocompleteOffset = new Point(6, 12);

	public int MaximumSuggestions = 100;

	public Color BackColor {
		get { return lb.BackColor; }
		set { lb.BackColor = value; }
	}

	public Color ForeColor {
		get { return lb.ForeColor; }
		set { lb.ForeColor = value; }
	}
	
	public bool AddUsingsOnTheFly {
		get { return _AddUsingsOnTheFly; }
		set {
			_AddUsingsOnTheFly = value;
			if (value)
				usings = GetUsings();
		}
	}

	#endregion

	#region Constructors

	public Autocomplete(TextBox textboxToAttach) {
		textbox = textboxToAttach;
		textbox.KeyDown += textbox_KeyDown;
		textbox.KeyUp += textbox_KeyUp; 
		textbox.LostFocus += textbox_LostFocus;
		textbox.MouseDown += textbox_MouseDown;

		lb = new ListBox {
			Dock = DockStyle.Fill
		};
		lb.MouseLeave += (sender, e) => HideSuggestions();
		lb.DoubleClick += lb_DoubleClick;

		af = new Form {
			Size = new Size(80, 200),
			FormBorderStyle = FormBorderStyle.None
		};
		af.Controls.Add(lb);
		
		textbox.ContextMenuStrip = cms;
		
		assembliesLocation = Environment.GetFolderPath(Environment.SpecialFolder.Windows).Trim('\\') +
			@"\Microsoft.NET\Framework" + (is64bitPC ? "64" : "") + @"\v4.0.30319\";

		LoadAssemblies();
	}

	#endregion
	
	#region usings management
	
	List<string> GetUsings() {
		string[] usings = textbox.Text.Split(new [] { "using " }, StringSplitOptions.None);
		var validUsings = new List<string>();
		foreach (var @using in usings) {
			string u = @using.Split(';')[0].Replace(" ", "");
			if (u.Equals(String.Empty))
				continue;
			
			foreach (var c in u)
				if (!IsAlpha(c))
					continue;
			
			if (textbox.Text.Replace(" ", "").Contains("using" + u + ";"))
				validUsings.Add(u);
		}
		return validUsings;
	}
	
	void AddUsing(string @using) {
		usings  = GetUsings();
		if (!usings.Contains(@using)) {
			usings.Add(@using);
			UpdateUsings();
		}
	}
	
	void UpdateUsings() {
		var lines = new List<string>(textbox.Lines);
		int removed = 0;
		for (int i = 0; i < lines.Count; i++) {
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
		
		if (textbox.InvokeRequired) { // if called from TaskFactory / async / other thread...
			textbox.Invoke(new MethodInvoker(() => {
 												int pos = textbox.SelectionStart += added - removed;
							                 	textbox.Text = newstr;
							                 	textbox.SelectionStart = pos;
								                 }));
		}
		else {
			int pos = textbox.SelectionStart += added - removed;
			textbox.Text = newstr;
			textbox.SelectionStart = pos;
		}
	}
	
	#endregion

	#region Load assemblies

	void LoadAssemblies() {
		new TaskFactory().StartNew(() => {
			// mscorelib
			foreach (var t in typeof(string).Assembly.GetTypes().Where(t => t.IsPublic))
				AddType(t);

			// System.dll
			foreach (var t in
			         Assembly.LoadFile(assembliesLocation + "System.dll")
			         .GetTypes().Where(t => t.IsPublic))
				AddType(t);
		});
	}

	/// <summary>
	/// Please keep in mind that a namespace is not an assembly.
	/// For example, you may use "System.Windows.Forms.dll" or specify the full path to the library
	/// </summary>
	/// <param name="assemblyFile">The assembly location</param>
	public void LoadAssembly(string assemblyFile) {
		LoadAssemblies(new [] { assemblyFile });
	}
	
	public void LoadAssemblies(string[] assemblyFiles) {
		new TaskFactory().StartNew(() => {
           	foreach (var assemblyFile in assemblyFiles) {
				string file = assemblyFile.Contains('\\') ? assemblyFile : assembliesLocation + assemblyFile;
	
				foreach (var t in
				         Assembly.LoadFile(file)
				         .GetTypes().Where(t => t.IsPublic))
				AddType(t);
           	}
		});
	}

	void AddType(Type type) {
		if (!loadedTypes.Contains(type))
			loadedTypes.Add(type);
	}

	#endregion

	#region Words management
	
	#region Get and remove last word

	string GetLastWord(int deep = 1) {
		// instead splitting only by ' '
		string[] split = Regex.Split(textbox.Text.Substring(0, textbox.SelectionStart),
		                             rnonalpha_nodot);

		string[] pts = split[split.Length - 1].Split('.');

		return pts.Length >= deep ? pts[pts.Length - deep] : "";
	}
	
	// TODO use me (GetLastWordIndexAndLength())
	// useful to select a whole word on right click and then determine the type and give some info...
	int[] GetLastWordIndexAndLength() {
		int i = textbox.SelectionStart;
		while (i >= 0 && IsAlpha(textbox.Text[i]))
			i--;
		
		int start = ++i; // ++i because it stops at ' ' (or another non-alpha char)
		
		while (i < textbox.Text.Length && IsAlpha(textbox.Text[i]))
			i++;
		
		int length = i - start;

		return new [] { start, length };
	}

	List<string> GetTypedFull(int maxDeep = 6) {
		var found = new List<string>();
		for (int i = 1; i <= maxDeep; i++) {
			var str = GetLastWord(i);
			if (str == "") break;
			found.Add(str);
		}

		found.Reverse();
		return found;
	}

	void RemoveLastWord(string replacewith = "") {
		int pos = textbox.SelectionStart;

		string[] split1 = Regex.Split(textbox.Text.Substring(0, textbox.SelectionStart), rnonalpha);

		int start = 0;
		for (int i = 0; i < split1.Length - 1; i++)
			start += split1[i].Length + 1;

		string[] split2 = Regex.Split(textbox.Text.Substring(start), rnonalpha);
		int end = start + split2[0].Length;

		textbox.Text = textbox.Text.Substring(0, start) + replacewith + textbox.Text.Substring(end);
		textbox.SelectionStart = start + replacewith.Length;
	}
	
	#endregion
	
	#region Is right after new?
	
	string IsRightAfterNew() {
		int i = textbox.SelectionStart;
		string returnme = "";
		
		if (i > 0) {
			i--;
			while (i > 0 && (IsAlpha(textbox.Text[i]) || textbox.Text[i] == ' ')) {
	       		i--;
			}
		}
		
		if (textbox.Text[i] == '.' && i > 0) {
			char c = textbox.Text[i - 1];
			
			int closingParenthesis = 0;
			int closingBrackets = 0;
			if (c == ')' || c == '}') {
				while (i >= 0) {
					c = textbox.Text[i];
					switch (c) {
						case ')':
							closingParenthesis++;
							break;
						case '(':
							closingParenthesis--;
							break;
						case '}':
							closingBrackets++;
							break;
						case '{':
							closingBrackets--;
							break;
					}
					if (c == 'n' && closingParenthesis == 0 && closingBrackets == 0)
						if (IsKeywordNew(i)) {
							i += 4; // "new ".Length
							while (IsAlpha(textbox.Text[i]))
								returnme += textbox.Text[i++];
							
							break;
						}
					i--;
				}
			}
		}
		
		return returnme.Trim();
	}
	
	bool IsKeywordNew(int index) {
		return textbox.Text.Substring(index, 4) == "new ";
	}
	
	#endregion
	
	#region Is In String

	bool IsInString() {
		string str = textbox.Text.Substring(0, textbox.SelectionStart);

		bool inNormal = false;
		bool inLiteral = false;
		bool skipNext = false;
		int idx = 0;

		foreach (var c in str) {
			if (skipNext) {
				idx++;
				skipNext = false;
				continue;
			}

			if (inNormal) {
				if (c == '\\')
					skipNext = true;
				else if (c == '"')
					inNormal = false;
			} else if (inLiteral) {
				if (c == '"')
						if (CheckNextQuote(str, idx))
							skipNext = true;
				else
					inLiteral = false;
			}			else {
				if (c == '"')
					inNormal = true;
				else if (c == '@' && CheckNextQuote(str, idx)) {
					inLiteral = skipNext = true;
				}
			}
			
			idx++;
		}
		
		return inNormal || inLiteral;
	}
	
	bool CheckNextQuote(string str, int index) {
		if (++index >= str.Length)
			return false;
		
		return str[index] == '"';
	}
	
	#endregion
	
	#region Sort by
	
    static IEnumerable<string> SortByLength(IEnumerable<string> e)
    {
		var sorted = from s in e
			     orderby s.Length ascending
			     select s;
		return sorted;
    }
    
    #endregion

	#endregion

	#region Types management

	Type FindTypeByName(string name) {
		List<Type> founds = loadedTypes.Where(t => t.Name == name).ToList();
		return founds.Count == 0 ? null : founds[0];
	}

	Type FindTypeByVariableName(string name) {
		string str = textbox.Text.Substring(0, textbox.SelectionStart);
		str = str.Replace(" ", "");

		int start = str.LastIndexOf(name + "=new", StringComparison.Ordinal);

		if (start > -1) {
			start += name.Length + 4; // 4 = "=new".Length
			int end = start;
			if (end < str.Length) {
				while (IsAlpha(str[end++]))
					if (end >= str.Length)
						break;
	
				return FindTypeByName(str.Substring(start, end - start - 1));
			}
		} else {
			start = str.LastIndexOf(name + "=", StringComparison.Ordinal);
			if (start > -1) {
				start += name.Length + 1; // 1 = "=".Length
				if (str.Length > start) {
					switch (str[start]) {
						case '"':
							return typeof(string);
						case '\'':
							return typeof(char);
						case 't':
						case 'f':
							return typeof(bool);
						default:
							if (IsDigit(str[start]))
							    return typeof(int);
							break;
					}
				}
			}
		}
		
		return null;
	}
	
	#endregion
	
	#region Autocomplete form
	
	#region Refresh suggestions
	
	void RefreshSuggesstions() {
		ClearSuggestions();
		
		var suggestions = new List<string>();
		
		string lsttype = GetLastWord(2);
		string lstword = GetLastWord().ToLower();
		string newk = IsRightAfterNew();
		
		if (newk.Length != 0) { // right after new something();
			Type t = FindTypeByName(newk);
			if (t != null)
				AddTypeSuggestions(t, lstword, ref suggestions, false);
		}
		else if (lsttype.Length != 0) { // in something.something
			Type t = FindTypeByName(lsttype);
			if (t != null) // static
				AddTypeSuggestions(t, lstword, ref suggestions, true);
			else {
				t = FindTypeByVariableName(lsttype);
				if (t != null) // instance
					AddTypeSuggestions(t, lstword, ref suggestions, false);
			}
		} else { // everything else
			int count = 0;
		
			foreach (var s in keywords.Where(s => s.StartsWith(lstword,
               StringComparison.OrdinalIgnoreCase)).ToList())
			{
				if (count++ > MaximumSuggestions)
					break;
				
				suggestions.Add(s);
			}
			var validTypes = loadedTypes.Where(t => t.Name.StartsWith(lstword,
              	StringComparison.OrdinalIgnoreCase)).ToList();
			foreach (var t in validTypes)
			{
				if (count++ > MaximumSuggestions)
					break;
				
				suggestions.Add(t.Name);
			}
		}
		
		AddSuggestions(SortByLength(suggestions));
		
		if (lb.Items.Count > 0)
			lb.SelectedIndex = 0;
		else
			HideSuggestions();
	}
	
	void AddTypeSuggestions(Type t, string lstword, ref List<string> suggestions, bool isStatic) {	
		int count = 0;
		
		foreach (var m in t.GetMethods().Where(m => m.IsPublic && m.IsStatic == isStatic &&
            m.Name.StartsWith(lstword, StringComparison.OrdinalIgnoreCase)).ToList())
		{
			if (count++ > MaximumSuggestions)
				break;
			
			suggestions.Add(m.Name);
		}
		
		foreach (var p in t.GetProperties().Where(p => p.GetGetMethod().IsStatic == isStatic &&
              p.GetGetMethod().IsPublic && p.Name.ToLower().StartsWith(lstword,
                      	         StringComparison.OrdinalIgnoreCase)).ToList())
		{
			if (count++ > MaximumSuggestions)
				break;
			
			suggestions.Add(p.Name);
		}
	}
	
	#endregion
	
	#region Add and clear suggestions
	
	void AddSuggestions(IEnumerable<string> suggestions) {
		lb.BeginUpdate();
		foreach (var suggestion in suggestions) {
			string s = suggestion;
			if (suggestion.Contains("`"))
				s = s.Split('`')[0];
			if (suggestion.StartsWith("get_", StringComparison.Ordinal))
				s = s.Replace("get_", "");
			if (suggestion.StartsWith("set_", StringComparison.Ordinal))
				s = s.Replace("set_", "");
			
			
			if (!lb.Items.Contains(s))
				lb.Items.Add(s);
		}
		lb.EndUpdate();
	}
	
	void ClearSuggestions() {
		lb.BeginUpdate();
		lb.Items.Clear();
		lb.EndUpdate();
	}
	
	#endregion
	
	#region Select suggestions
	
	bool SelectSuggestion(string toadd = "") {
		if (lb.Items.Count > 0 && af.Visible) {
			string selected = (string)lb.SelectedItem;
			RemoveLastWord(selected + toadd);
			
			if (AddUsingsOnTheFly)
				new TaskFactory().StartNew(() => AddUsing(FindTypeByName(selected).Namespace));
			
			HideSuggestions();
			
			return true;
		}
		return false;
	}
	
	bool MoveUp() {
		if (!af.Visible)
			return false;
		
		if (lb.SelectedIndex > 0)
			lb.SelectedIndex--;
		
		return true;
	}
	
	bool MoveDown() {
		if (!af.Visible)
			return false;
		
		if (lb.SelectedIndex < lb.Items.Count - 1)
			lb.SelectedIndex++;
		
		return true;
	}
	
	#endregion
	
	#region Show / Hide
	
	void ShowSuggestions() {
		if (!af.Visible)
			ShowInactiveTopmost(af);
		
		RefreshLocation();
		RefreshSuggesstions();
	}
	
	void HideSuggestions() {
		if (!new Rectangle(lb.PointToScreen(lb.Location), lb.Size).Contains(Cursor.Position)
		   || lb_doubleclick)
		{
			af.Hide();
			ClearSuggestions();
			lb_doubleclick = false;
		}
	}
	
	#endregion
	
	#region Location
	
	void RefreshLocation() {
		if (af.Visible)
			af.Location = GetPointFromCursorPosition();
	}
	
	Point GetPointFromCursorPosition() {
		Point pt = textbox.PointToScreen(textbox.GetPositionFromCharIndex
        	(textbox.SelectionStart > 0 ? textbox.SelectionStart - 1 : 0));
		return new Point(pt.X + AutocompleteOffset.X, pt.Y + AutocompleteOffset.Y);
	}
	
	#endregion
	
	#endregion
	
	#region Magic keys
	
	List<Keys> magicKeys = new List<Keys> {
		Keys.Space, Keys.Tab, Keys.Enter, Keys.OemPeriod
	};
	
	string GetKeyStr(Keys key) {
		switch (key) {
			case Keys.Space:
				return " ";
			case Keys.OemPeriod:
				return ".";
			default:
				return "";
		}
	}
	
	#endregion
	
	#region Is Alpha or Digit?
	
	bool IsAlpha(Keys key) {
		string k = key.ToString();
		return k.Length <= 1 && Regex.IsMatch(k, "\\w");
	}
	
	bool IsAlpha(char c) {
		return Regex.IsMatch(c.ToString(), "\\w");
	}
	
	bool IsDigit(char c) {
		int a = 0;
		return Int32.TryParse(c.ToString(), out a);
	}
	
	#endregion
	
	#region Events
	
	void textbox_KeyDown(object sender, KeyEventArgs e)
	{	
		if (magicKeys.Contains(e.KeyCode))
			e.Handled = e.SuppressKeyPress = SelectSuggestion(GetKeyStr(e.KeyCode));
		else if (e.KeyCode == Keys.Up)
			e.Handled = e.SuppressKeyPress = MoveUp();
		else if (e.KeyCode == Keys.Down)
			e.Handled = e.SuppressKeyPress = MoveDown();
		else if (!IsInString() && IsAlpha(e.KeyCode))
			ShowSuggestions();
		else
			HideSuggestions();
	}

	void textbox_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
			return;
		
		RefreshLocation();
		RefreshSuggesstions();
	}

	void textbox_MouseDown(object sender, MouseEventArgs e)
	{
		HideSuggestions();
	}
	
	void lb_DoubleClick(object sender, EventArgs e)
	{
		lb_doubleclick = true;
		SelectSuggestion();
	}
	
	void textbox_LostFocus(object sender, EventArgs e)
	{
		HideSuggestions();
	}
	
	#endregion
	
	#if DEBUG
	
	static void log(object msg = null) {
		System.Diagnostics.Debug.WriteLine(msg ?? "---------------------");
	}
	
	#endif
	
}