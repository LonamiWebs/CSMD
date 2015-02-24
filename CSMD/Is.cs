/** Made by Lonami Exo
 * (C) LonamiWebs 2015
 * 22-02-2015 18:50 */

/// <summary>
/// A class for determining whether something is or not with the maximum performance
/// </summary>
public static class Is
{
	#region Consts
	
	const int LowerCaseStart = 97;
	const int LowerCaseEnd = 122;
	
	const int UpperCaseStart = 65;
	const int UpperCaseEnd = 90;
	
	const int AccentsStart = 192;
	const int AccentsEnd = 255;
	
	const int NumericStart = 48;
	const int NumericEnd = 57;
	
	#endregion
	
	#region Is Alpha

	/// <summary>
	/// Checks whether a text is or not alpha
	/// </summary>
	/// <param name="text">The text to check</param>
	/// <returns>True if it has only alpha characters</returns>
	public static bool Alpha(string text)
	{
		foreach (var c in text) {
			if (!Alpha(c))
				return false;
		}
		return true;
	}

	/// <summary>
	/// Checks whether a character is or not alpha
	/// </summary>
	/// <param name="c">The character to check</param>
	/// <returns>True if it is alpha</returns>
	public static bool Alpha(char c)
	{
		int val = (int)c;
		return (val >= LowerCaseStart && val <= LowerCaseEnd)
			|| (val >= UpperCaseStart && val <= UpperCaseEnd)
			|| (val >= AccentsStart && val <= AccentsEnd);
	}
	
	#endregion
	
	#region Is Non Alpha

	/// <summary>
	/// Checks whether a text isn't or is alpha.
	/// Please consider using this instead of !Is.Alpha(...)
	/// </summary>
	/// <param name="text">The text to check</param>
	/// <returns>True if it has only non-alpha characters</returns>
	public static bool NonAlpha(string text)
	{
		foreach (var c in text)
			if (Alpha(c))
				return false;
		return true;
	}
	
	#endregion
	
	#region Is Numeric

	/// <summary>
	/// Checks whether a text is or not numeric
	/// </summary>
	/// <param name="text">The text to check</param>
	/// <returns>True if it's numeric</returns>
	public static bool Numeric(string text)
	{
		foreach (var c in text) {
			if (!Numeric(c))
				return false;
		}
		return true;
	}
	
	/// <summary>
	/// Checks whether a character is or not numeric
	/// </summary>
	/// <param name="c">The character to check</param>
	/// <returns>True if it's numeric</returns>
	public static bool Numeric(char c)
	{
		int val = (int)c;
		return (val >= NumericStart && val <= NumericEnd);
	}
	
	#endregion
	
	#region Is Non Numeric	

	/// <summary>
	/// Checks whether a text isn't or is numeric.
	/// Please consider using this instead of !Is.Numeric(...)
	/// </summary>
	/// <param name="text">The text to check</param>
	/// <returns>True if it's not numeric</returns>
	public static bool NonNumeric(string text)
	{
		foreach (var c in text)
			if (Numeric(c))
				return false;
		return true;
	}
	
	#endregion	
}