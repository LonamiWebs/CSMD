using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Linq;

public partial class MethodControl : UserControl
{
	readonly Color ControlColor = Color.FromArgb(240, 240, 240);
	const string ControlColorHex = "F0F0F0";
	
	const string BaseHTMLS = @"<!DOCTYPE html><html><head><style>body{background-color:#F0F0F0;}p{font-family:Consolas,""Lucida Console"",Monaco,monospace;font-size:12px;}.other{color:#A22;}.modifier{font-weight:bold;color:blue;}.type{color:red;}.refout{color:#F19;}</style></head><body><p></body></html>";
	const string BaseHTMLE = @"</p></body></html>";
	
	List<MethodInfo> MIs = new List<MethodInfo>();
	int Idx = -1;
	
	readonly int ScWidth = Screen.PrimaryScreen.Bounds.Width;
	
	public MethodControl()
	{
		InitializeComponent();
		BackColor = ControlColor;
		methodWB.DocumentText = BaseHTMLS + BaseHTMLE;
		methodWB.Refresh();
	}
	
	public void SetMethods(List<MethodInfo> mi)
	{
		if (MIs == mi)
			return;
		
		MIs = mi;
		Idx = 0;
		RefreshInfo();
	}
	
	public void MoveUp()
	{
		int idx = Idx;
		idx--;
		if (idx < 0)
			idx = MIs.Count - 1;
		
		Idx = idx;
		
		RefreshInfo();
	}
	
	public void MoveDown()
	{
		int idx = Idx;
		idx++;
		if (idx >= MIs.Count)
			idx = 0;
		
		Idx = idx;
		
		RefreshInfo();
	}
	
	void RefreshInfo()
	{
		if (Idx < 0 || Idx >= MIs.Count)
			return;
		
		countL.Text = (Idx + 1) + "/" + MIs.Count;
		SetHTML(BaseHTMLS + GetSignature(MIs[Idx]) + BaseHTMLE);
		
		Width = ScWidth - Location.X;
	}
	
	#region Web Browser Utils
	
	void SetHTML(string html)
	{
		methodWB.Navigate("about:blank");
		while (methodWB.Document == null || methodWB.Document.Body == null)
		    Application.DoEvents();
		methodWB.Document.OpenNew(true).Write(html);
		methodWB.Refresh();
	}
	
	#endregion
	
	#region Method Info utils
	
	public static string GetSignature(MethodInfo method, bool callable = false)
    {
        var firstParam = true;
        var sb = new StringBuilder();
        
        if (!callable)
        {
            if (method.IsPublic)
                sb.Append("<span class=\"modifier\">public</span> ");
            else if (method.IsPrivate)
                sb.Append("<span class=\"modifier\">private</span> ");
            else if (method.IsAssembly)
                sb.Append("<span class=\"modifier\">internal</span> ");
            
            if (method.IsFamily)
                sb.Append("<span class=\"modifier\">protected</span> ");
            
            if (method.IsStatic)
				sb.Append("<span class=\"other\">static</span> ");
            
            sb.Append("<span class=\"type\">" + TypeName(method.ReturnType) + "</span> ");
        }
        sb.Append(method.Name);

        // Add method generics
        if(method.IsGenericMethod)
        {
            sb.Append("<");
            foreach(var g in method.GetGenericArguments())
            {
                if (firstParam)
                    firstParam = false;
                else
                    sb.Append(", ");
                sb.Append(TypeName(g));
            }
            sb.Append(">");
        }
        
        sb.Append("(");
        firstParam = true;
        var secondParam = false;
        
        foreach (var param in method.GetParameters())
        {
            if (firstParam)
            {
                firstParam = false;
                if (method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))
                {
                    if (callable)
                    {
                        secondParam = true;
                        continue;
                    }
                    
                	sb.Append("<span class=\"modifier\">this</span> ");
                }
            }
            else if (secondParam)
                secondParam = false;
            else
                sb.Append(", ");
            
            if (param.ParameterType.IsByRef)
                sb.Append("<span class=\"refout\">ref</span> ");
            else if (param.IsOut)
                sb.Append("<span class=\"refout\">out</span> ");
            
            if (!callable)
            {
                sb.Append(TypeName(param.ParameterType));
                sb.Append(' ');
            }
            sb.Append(param.Name);
        }
        sb.Append(")");
        
        return sb.ToString();
    }

    /// <summary>
    /// Get full type name with full namespace names
    /// </summary>
    /// <param name="type">Type. May be generic or nullable</param>
    /// <returns>Full type name, fully qualified namespaces</returns>
    public static string TypeName(Type type)
    {
        var nullableType = Nullable.GetUnderlyingType(type);
        if (nullableType != null)
            return nullableType.Name + "?";

        if (!type.IsGenericType)
            switch (type.Name)
            {
                case "String": return "string";
                case "Int32": return "int";
                case "Decimal": return "decimal";
                case "Object": return "object";
                case "Void": return "void";
                
                default: return type.Name;
            }

        var sb = new StringBuilder(type.Name.Substring(0, type.Name.IndexOf('`')));
        
        sb.Append('<');
        var first = true;
        foreach (var t in type.GetGenericArguments())
        {
            if (!first)
                sb.Append(',');
            
            sb.Append(TypeName(t));
            first = false;
        }
        sb.Append('>');
        
        return sb.ToString();
    }
	
	#endregion
}
