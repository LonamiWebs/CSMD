using CSMD.Properties;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace CSMD
{
    public class Compiler
    {
        string[] Files = new string[0];

        string[] ReferencedAssemblies;
        string ExecutablePath, NETVersion;
        bool DeleteOnExit, RandomName;

        public Compiler() { Init(); }
        public Compiler(string[] files) {
            Files = files;
            Init();
        }

        void Init() {
            ReferencedAssemblies = Program.StringCollectionToArray(Settings.Default.ReferencedAssemblies);
            ExecutablePath = Settings.Default.ExecutablePath;
            NETVersion = Settings.Default.NETVersion;
            DeleteOnExit = Settings.Default.DeleteOnExit;
            RandomName = Settings.Default.RandomName;
        }


        public void Compile(string code = "") {

            var provider = new CSharpCodeProvider(new Dictionary<String, String>
            { { "CompilerVersion", NETVersion } });

            var cp = new CompilerParameters {
                GenerateInMemory = false,
                GenerateExecutable = true,
                IncludeDebugInformation = false,
                OutputAssembly = RandomName ?
                    Path.GetFileNameWithoutExtension(ExecutablePath) + "-" + Path.GetRandomFileName() + ".exe"
                    : ExecutablePath
            };

            cp.ReferencedAssemblies.AddRange(ReferencedAssemblies);

            if (Files.Length > 0) {
                foreach (var file in Files)
                    if (File.Exists(file))
                        Compile(cp, provider, File.ReadAllText(file));
            } else
                Compile(cp, provider, code);
        }

        void Compile(CompilerParameters cp, CodeDomProvider provider, string code) {
        	
        	const string basecode = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Foo {
	public class Bar {
		@main
	}
}";
        	
        	const string main = @"static void Main(string[] args) {
			@code
		}";
        	
        	if (!code.Contains("namespace Foo") && !code.Contains("public class bar"))
        		if (!code.Contains("static void Main"))
        			code = basecode.Replace("@main", main).Replace("@code", code);
        		else
        			code = basecode.Replace("@main", code);
        	
            CompilerResults cr = provider.CompileAssemblyFromSource(cp, code);

            if (cr.Errors.Count > 0)
            {
                var errors = new List<string>();

                foreach (CompilerError ce in cr.Errors)
                    errors.Add((ce.IsWarning ? "Warning " : "Error ") + ce.ErrorNumber + " [Line " + ce.Line
                        + " | Column " + ce.Column + "] " + ":\r\n " + ce.ErrorText);

                MessageBox.Show("There are some errors in your code:\r\n\r\n"
                    + String.Join("\r\n\r\n", errors),
                "Compilation errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (RandomName)
                    Process.Start(cp.OutputAssembly);
                else
                {
                    Process.Start(cp.OutputAssembly).WaitForExit();

                    if (DeleteOnExit)
                        File.Delete(cp.OutputAssembly);
                }
            }
        }
    }
}
