using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public static class Utils
{
    public static bool ValidFilePath(string path)
    {
    	List<char> invalidChars = Path.GetInvalidFileNameChars().ToList();
    	if (path.Contains("\\") && path.Count(c => c == ':') == 1) {
    		invalidChars.Remove('\\');
    		invalidChars.Remove(':');
    		return path.Trim('\\') == path && path.IndexOfAny(invalidChars.ToArray()) < 0;
    	}
    	return !String.IsNullOrWhiteSpace(path) && path.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
    }
}

/** Made by Lonami Exo
 * 31 - January - 2015
 * (C) LonamiWebs - */
public static class ScrollUtils
{
	#region Consts and externs

	const int EM_LINESCROLL = 0x00B6;
	
	[DllImport("user32.dll")]
	static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
	[DllImport("user32.dll")]
	static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
	
	[DllImport("user32.dll")]
	static extern int GetScrollPos(IntPtr hWnd, int nBar);
	
	#endregion
	
	#region Public static methods
	
	/// <summary>
	/// Sets the given scroll position to the desired TextBox
	/// </summary>
	/// <param name="tb">The TextBox</param>
	/// <param name="position">The new scroll position</param>
	/// <param name="orientation">The scroll orientation</param>
	public static void SetScrollPosition(TextBox tb, int position,
                                 Orientation orientation = Orientation.Vertical)
	{	
		var handle = GetHandle(tb);
		int add = position - GetScrollPosition(tb, orientation);
		
		SetScrollPos(handle, (int)orientation, add, true); // this work w/o this but...
		SendMessage(handle, EM_LINESCROLL, 0, add);
	}
	
	/// <summary>
	/// Adds (or substracts) the given scroll position to the desired TextBox
	/// </summary>
	/// <param name="tb">The TextBox</param>
	/// <param name="position">The scroll position to be added</param>
	/// <param name="orientation">The scroll orientation</param>
	public static void AddScrollPosition(TextBox tb, int position,
                                 Orientation orientation = Orientation.Vertical)
	{
		var handle = GetHandle(tb);
		
		SetScrollPos(handle, (int)orientation, position, true); // this work w/o this but...
		SendMessage(handle, EM_LINESCROLL, 0, position);
	}
	
	/// <summary>
	/// Gets the current scroll position from the specified TextBox
	/// </summary>
	/// <param name="tb">The TextBox</param>
	/// <param name="orientation">The scroll orientation</param>
	/// <returns></returns>
	public static int GetScrollPosition(TextBox tb,
                            	Orientation orientation = Orientation.Vertical)
	{
		var handle = GetHandle(tb);
		
		return GetScrollPos(handle, (int)orientation);
	}
	
	static IntPtr GetHandle(TextBox tb) {
		IntPtr handle = IntPtr.Zero;
		
		if (tb.InvokeRequired) {
			tb.Invoke(new MethodInvoker(() => handle = tb.Handle));
		} else
			handle = tb.Handle;
		
		return handle;
	}
	
	#endregion
}
