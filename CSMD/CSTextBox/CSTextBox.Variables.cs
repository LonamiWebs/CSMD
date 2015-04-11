using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

public partial class CSTextBox
{
	#region Variables and properties
	
	#region Consts
	
	// Overrides
	const int ACode = 65, ZCode = 90;
	const int Code0 = 48, Code9 = 57;
	
	// Text
	const int LowerCaseStart = 97, LowerCaseEnd = 122;
	const int UpperCaseStart = 65, UpperCaseEnd = 90;
	const int AccentsStart = 192, AccentsEnd = 255;
	const int NumericStart = 48, NumericEnd = 57;
	
	// Types
	const string VariablesRegexS = @"(?:\b[^\(]\s*\w+)+\s+(\w+)(?:\s*,\s*(\w+))*\b";
	
	// TODO (UseMe) - ParamethersRegex
	const string ParamethersRegexS = @"(?:\b\(\s*\w+\s+(\w+)(?:\s*,\s*\w+\s+(\w+))\b";
	
	// This won't work, cause would match: method(type name, type name) -> name, type
	// const string VariablesAndParamethersRegexWRONG = @"(?:\s*\w+)+\s+(\w+)(?:\s*,\s*(\w+))*";
	
	#endregion
	
	#region Private
	
	// Main
	AutocompleteForm Autocomplete = new AutocompleteForm();
	
	static bool Is64BitPC = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));
	
	// Overrides
	bool MustRefresh, InStrOrDeclaring;
	
	// Words
	readonly char[] ValidSeparators = new char[] { '.', ' ', '\t', '\r', '\n' };
	bool FoundParenthesis;
	
	// Indent
	const int SpacesAsTab = 4;
	
	int LineTabs
	{
		get {
			
			int tabs = 0, i = SelectionStart - 1;
			
			while (i > -1)
			{
				switch (Text[i])
				{
					case '\n':
						return tabs;
					case '\t':
						tabs++;
						break;
					default:
						tabs = 0;
						break;
				}
				
				i--;
			}
			
			return tabs;
		}
	}
	
	// Autocomplete
	HashSet<string> LoadedNamespaces = new HashSet<string>();
	HashSet<Type> LoadedTypes = new HashSet<Type>();
	
	readonly string AssembliesLocation = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
		.Trim('\\') + @"\Microsoft.NET\Framework" + (Is64BitPC ? "64" : "") + @"\v4.0.30319\";
	
	volatile bool LoadingAssemblies;
	
	readonly string[] Keywords = { "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while", "add", "alias", "ascending", "async", "await", "descending", "dynamic", "from", "get", "global", "group", "into", "join", "let", "orderby", "partial", "remove", "select", "set", "value", "var", "where", "yield" };
	
	/* "AS" stands for And Similars */
	readonly string[] TypesAS = { "bool", "byte", "char", "class", "decimal", "double", "enum", "event", "float", "goto", "int", "interface", "long", "namespace", "object", "sbyte", "short", "string", "struct", "uint", "ulong", "ushort", "void", "var" };
	readonly string[] ModifiersAS = { "abstract", "as", "base", "checked", "const", "delegate", "explicit", "extern", "fixed", "implicit", "in", "is", "out", "override", "params", "private", "public", "readonly", "ref", "return", "sealed", "stackalloc", "static", "throw", "unchecked", "unsafe", "virtual", "volatile", "async", "await", "dynamic", "global", "partial", "yield", "new" };
	
	readonly string[] Instructions = { "for", "foreach", "if", "using", "switch", "case", "while", "catch", "lock", "sizeof", "typeof" };
	
	// Types
	static readonly Regex VariablesRegex = new Regex(VariablesRegexS, RegexOptions.Compiled);
	
	#endregion
	
	#region Public
	
	// Main
	public int MaximumSuggestions = 100;
	public Point AutocompleteOffset = new Point(6, 12);
	
	public bool AutocompletionEnabled;
	
	#endregion
	
	#endregion
}