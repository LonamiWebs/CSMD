using System;
using System.Linq;

public partial class CSTextBox
{
	#region Declaring
	
	bool IsDeclaring()
	{
		var type = GetLastTypeIfDeclaring();
		return type.Length > 0 && !ModifiersAS.Contains(type) && type != "using";
	}
	
	Type GetLastDeclaredType()
	{
		return null;
	}
	
	#endregion
}