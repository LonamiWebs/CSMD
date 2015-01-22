// CSMD: A simple and quick CSharp command prompt for Windows
// Copyright (C) 2014  Lonami Exo
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using CSMD.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CSMD
{
    public partial class MainF : Form
    {
        public static Compiler c = new Compiler();

        #region Setup
        
        Autocomplete ac;

        public MainF()
        {
        	InitializeComponent();
        	
        	compileTSSB.DropDownItems.AddRange(new ToolStripItem[]
            { settingsTSMI, saveTSMI, tss, spamTSMI});
        	
        	if (Settings.Default.Autocompletion) {
	        	ac = new Autocomplete(consoleTB) {
	        		BackColor = Color.Black,
	        		ForeColor = Color.Lime,
	        		AddUsingsOnTheFly = Settings.Default.ImportsOnTheFly
	        	};
	        	
	    		ac.LoadAssemblies(Program.StringCollectionToArray(Settings.Default.ReferencedAssemblies));
        	}
            
            if (Settings.Default.ExecutablePath == "") {
                Settings.Default.ExecutablePath = Path.GetTempPath().Trim('\\') + "\\csmc.exe";
                Settings.Default.Save();
            }

            Program.AssociateFileType("CSMD", "cst", "CSMD file", false, 1);

            infoTSSL.Text += Application.ProductVersion;
        }
        
        void MainF_Load(object sender, EventArgs e) {
            consoleTB.Select(170, 0);
        }

        #endregion

        void consoleTB_KeyDown(object sender, KeyEventArgs e) {
            int i = consoleTB.SelectionStart;
            int l = consoleTB.GetLineFromCharIndex(i);
            int t = LineTabs(l);

            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB " + t + "}");

            //bfss = i;

            //if (e.KeyCode == Keys.Home)
            //    SendKeys.Send("{RIGHT " + t + "}");

            if (e.Control && e.KeyCode == Keys.Enter) {
                e.Handled = true;
                e.SuppressKeyPress = true;
                Compile();
            }
        }

        int bfss = -1; // before selection start

        void consoleTB_KeyUp(object sender, KeyEventArgs e)
        {
            //int i = consoleTB.SelectionStart;
            //int l = consoleTB.GetLineFromCharIndex(i);
            //int t = LineTabs(l);

            //if (e.KeyCode == Keys.Home)
            //    if (bfss == i)
            //        SendKeys.Send("{LEFT " + t + "}");
        }


        int LineTabs(int lineIndex)
        {
            string line = consoleTB.Text.Split('\n')[lineIndex];
            string fl = Regex.Split(line, "[^\t]+")[0];
            return fl.Count(c => c == '\t');
        }

        #region Compile

        void compileTSSB_ButtonClick(object sender, EventArgs e) {
            Compile();
        }

        void Compile() { c.Compile(consoleTB.Text); }

        #endregion

        #region Status strip

        void settingsTSMI_Click(object sender, EventArgs e)
        { new SettingsF().Show(); }

        void saveTSMI_Click(object sender, EventArgs e) {
            if (codeSFD.ShowDialog() == DialogResult.OK)
                File.WriteAllText(codeSFD.FileName, consoleTB.Text, Encoding.UTF8);
        }

        void spamTSMI_Click(object sender, EventArgs e)
        { Process.Start("http://lonamiwebs.tk"); }

        #endregion

        public static void m(object msg) { System.Diagnostics.Debug.WriteLine(msg == null ? "null" : msg.ToString()); }

    }
}
