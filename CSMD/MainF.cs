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
using CSMD.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSMD
{
    public partial class MainF : Form
    {
        public static Compiler c = new Compiler();
        
        int tabs = -1;

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
            
        	if (!ValidFilePath(Settings.Default.ExecutablePath))
        	{
        		Settings.Default.ExecutablePath = Path.Combine(Path.GetTempPath(), "csmc.exe");
                Settings.Default.Save();
            }

            Program.AssociateFileType("CSMD", "cst", "CSMD file", false, 1);

            infoTSSL.Text += Application.ProductVersion;
        }
        
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
        
        void MainF_Load(object sender, EventArgs e) {
            consoleTB.Select(170, 0);
        }

        #endregion
        
        void consoleTB_KeyDown(object sender, KeyEventArgs e)
        {
        	if (e.KeyCode == Keys.Enter)
	        	if (e.Control) {
	                e.Handled = e.SuppressKeyPress = true;
	                Compile();
	            }
        		else
        			tabs = LineTabs;
        }

        void consoleTB_KeyUp(object sender, KeyEventArgs e)
        {
        	if (tabs > -1)
        	{
        		int lpos = consoleTB.SelectionStart;
        		consoleTB.Text = consoleTB.Text.Insert(lpos, new String('\t', tabs));
        		consoleTB.SelectionStart += lpos + tabs;
        		tabs = -1;
        	}
        }


        int LineTabs {
        	get {
	        	int ts = 0;
	        	int i = consoleTB.SelectionStart;
	        	while (--i > -1)
	        	{
	        		if (consoleTB.Text[i] == '\n')
	        			break;
	        		if (consoleTB.Text[i] == '\t')
	        			ts++;
	        		else
	        			ts = 0;
	        	}
	        	return ts;
        	}
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

    }
}
