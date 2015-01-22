using Microsoft.Win32;
using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CSMD
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
                new Compiler(args).Compile();
            else {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainF());
            }
        }

        public static string[] StringCollectionToArray(StringCollection stringCollection) {
            var array = new string[stringCollection.Count];
            for (int i = 0; i < array.Length; i++)
                array[i] = stringCollection[i];
            return array;
        }

        [DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        public static void AssociateFileType(string programName, string extension, string fileInfo, bool force = false, int iconIndex = 0)
        {
            if (force || Registry.GetValue("HKEY_CLASSES_ROOT\\" + programName, String.Empty, String.Empty) == null) {
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\" + programName, "", fileInfo);
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\" + programName + "\\DefaultIcon", "",
                    "\"" + Application.ExecutablePath + "\"" + "," + iconIndex);
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\" + programName + "\\shell\\open\\command", "",
                    "\"" + Application.ExecutablePath + "\"" + " \"%1\"");
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\Classes\\." + extension, "", programName);

                // this call notifies Windows that it needs to redo the file associations and icons
                SHChangeNotify(0x08000000, 0x2000, IntPtr.Zero, IntPtr.Zero);
            }
        }
    }
}
