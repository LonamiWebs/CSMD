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
    	// The compiler
        public static Compiler c = new Compiler();

        #region Setup

        public MainF()
        {
        	InitializeComponent();
        	
        	// Set up the autocompletion
        	consoleCSTB.AutocompletionEnabled = Settings.Default.Autocompletion;
        	
        	consoleCSTB.LoadAssemblies(Program.StringCollectionToArray(Settings.Default.ReferencedAssemblies));
            
        	// Check if the compiled output has a valid path
        	if (!Utils.ValidFilePath(Settings.Default.ExecutablePath))
        	{
        		// If it doesn't, reset
        		Settings.Default.ExecutablePath = Path.Combine(Path.GetTempPath(), "csmc.exe");
                Settings.Default.Save();
            }

        	// Check for program association with .cst files
            Program.AssociateFileType("CSMD", "cst", "CSMD file", false, 1);

            // Append version to the info label
            infoTSSL.Text += Application.ProductVersion;
        }
        
        void MainF_Load(object sender, EventArgs e)
        {
        	consoleCSTB.SelectionStart = 165;
        }

        #endregion
        
        void consoleCSTB_KeyDown(object sender, KeyEventArgs e)
        {
        	if (e.KeyCode == Keys.Enter && e.Control)
        	{
                e.Handled = e.SuppressKeyPress = true;
                Compile();
            }
        }

        #region Compile

        void compileTSSB_ButtonClick(object sender, EventArgs e) {
            Compile();
        }

        void Compile() { c.Compile(consoleCSTB.Text); }

        #endregion

        #region Status strip

        void settingsTSMI_Click(object sender, EventArgs e)
        { new SettingsF().Show(); }

        void saveTSMI_Click(object sender, EventArgs e)
        {
            if (codeSFD.ShowDialog() == DialogResult.OK)
                File.WriteAllText(codeSFD.FileName, consoleCSTB.Text, Encoding.UTF8);
        }

        void spamTSMI_Click(object sender, EventArgs e)
        { Process.Start("http://lonamiwebs.tk"); }

        #endregion

    }
}
