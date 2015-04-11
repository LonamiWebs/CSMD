using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

public partial class CSTextBox : RichTextBox 
{
	// Keep scroll: ScrollToCaret or http://stackoverflow.com/questions/1743448/auto-scrolling-text-box-uses-more-memory-than-expected?
	
	#region Constructors
	
	/// <summary>
	/// Initializes a new CSTextBox instance
	/// </summary>
	public CSTextBox()
	{
		AcceptsTab = true;
		WordWrap = false;
		
		Autocomplete.RequestSuggestion += SelectSuggestion;
		
		// Almost 6 MB of RAM - That's nothing! ;)
		LoadAssemblies(new string[] { "mscorlib.dll", "System.dll" });
	}
	
	#endregion
	
	#region Load autocomplete
	
	/// <summary>
	/// Loads the specified assemblies
	/// </summary>
	/// <param name="assemblyFiles">Assemblies</param>
	public void LoadAssemblies(params string[] assemblyFiles)
	{
		new TaskFactory().StartNew(() =>
       	{                           	
           	while (LoadingAssemblies) { /* wait */ }
           	
           	LoadingAssemblies = true;
           	
           	foreach (string asm in assemblyFiles)
           	{
           		foreach (var t in Assembly.LoadFile(asm.Contains("\\") ? asm : AssembliesLocation + asm)
           		         .GetTypes().Where(t => t.IsPublic))
           		{
           			LoadedTypes.Add(t);
       				LoadedNamespaces.Add(t.Namespace);
           		}
           	} 
           	
           	LoadingAssemblies = false;
		});
	}
	
	#endregion
	
	#region Others
	
	public static void log(params object[] msgs) {
		System.Diagnostics.Debug.WriteLine(String.Join(", ", msgs));
	}
	
	#endregion
}	