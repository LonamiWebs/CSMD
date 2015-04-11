using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	public static class KeysMethods
	{	
		/// <summary>
		/// Checks if a KeyEventArgs is equals to the specified char
		/// </summary>
		/// <param name="kea">The KeyEventArgs to compare</param>
		/// <param name="c">The char to compare</param>
		/// <returns>True if are equal</returns>
		public static bool EqualsChar(this KeyEventArgs kea, char c)
		{ return kea.GetFullKey() == c.ToVirtualKey(); }
	    
		/// <summary>
		/// Retrieves the full key (if shift, control or alt is being held) from a KeyEventArgs
		/// </summary>
		/// <param name="kea">The KeyEventArgs from where to extract the key</param>
		/// <returns>The full key</returns>
	    public static Keys GetFullKey(this KeyEventArgs kea)
	    { return GetFullKey(kea.KeyCode, kea.Shift, kea.Control, kea.Alt); }
	    
	    /// <summary>
	    /// Retrieves the full key (if shift, control or alt is being held) from a KeyEventArgs
	    /// </summary>
	    /// <param name="key">The key pressed</param>
	    /// <param name="shift">If shift being held?</param>
	    /// <param name="control">Is control being held?</param>
	    /// <param name="alt">Is alt being held?</param>
	    /// <returns>The full key</returns>
	    public static Keys GetFullKey(Keys key, bool shift, bool control, bool alt)
	    {
	        if (shift) key |= Keys.Shift;
	        if (control) key |= Keys.Control;
	        if (alt) key |= Keys.Alt;
	        
	        return key;
	    }
	}
	
	public static class CharMethods
	{
		// Static extern
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		static extern short VkKeyScan(char ch);
		
		/// <summary>
		/// Converts a char to a <see cref="KeysMethods.GetFullKey">full virtual key</see>
		/// </summary>
		/// <param name="c">The char to convert</param>
		/// <returns>The full key</returns>
		public static Keys ToVirtualKey(this char c)
		{
	        short vkey = VkKeyScan(c);
	        var retval = (Keys)(vkey & 0xff);
	        int modifiers = vkey >> 8;
	        if ((modifiers & 1) != 0) retval |= Keys.Shift;
	        if ((modifiers & 2) != 0) retval |= Keys.Control;
	        if ((modifiers & 4) != 0) retval |= Keys.Alt;
	        return retval;
		}
	}
}
