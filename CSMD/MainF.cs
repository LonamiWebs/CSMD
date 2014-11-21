using CSMD.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
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

        string[] Args = new string[0];

        public MainF(string[] args = null)
        {
            InitializeComponent();

            if (args != null) {
                Visible = false;
                Args = args;
            }
            else // This takes so, so long in InitializeComponent();
                compileTSSB.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
                { settingsTSMI, saveTSMI, tss, spamTSMI});

            if (Settings.Default.ExecutablePath == "") {
                Settings.Default.ExecutablePath = Path.GetTempPath().Trim('\\') + "\\csmc.exe";
                Settings.Default.Save();
            }

            Program.AssociateFileType("CSMD", "cst", "CSMD file", false, 1);

            infoTSSL.Text += Application.ProductVersion;
        }

        private void MainF_Load(object sender, EventArgs e) {
            consoleTB.Select(170, 0);
        }

        #endregion

        

        #region Editor

        private void consoleTB_KeyDown(object sender, KeyEventArgs e) {

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

        private void consoleTB_KeyUp(object sender, KeyEventArgs e)
        {
            //int i = consoleTB.SelectionStart;
            //int l = consoleTB.GetLineFromCharIndex(i);
            //int t = LineTabs(l);

            //if (e.KeyCode == Keys.Home)
            //    if (bfss == i)
            //        SendKeys.Send("{LEFT " + t + "}");
        }


        private int LineTabs(int lineIndex)
        {
            string line = consoleTB.Text.Split('\n')[lineIndex];
            string fl = Regex.Split(line, "[^\t]+")[0];
            return fl.Count(c => c == '\t');
        }

        #endregion

        #region Compile

        private void compileTSSB_ButtonClick(object sender, EventArgs e) {
            Compile();
        }

        void Compile() { c.Compile(consoleTB.Text); }

        #endregion

        #region Status strip

        private void settingsTSMI_Click(object sender, EventArgs e)
        { new SettingsF().Show(); }

        private void saveTSMI_Click(object sender, EventArgs e) {
            if (codeSFD.ShowDialog() == DialogResult.OK)
                File.WriteAllText(codeSFD.FileName, consoleTB.Text, Encoding.UTF8);
        }

        private void spamTSMI_Click(object sender, EventArgs e)
        { Process.Start("http://lonamiwebs.tk"); }

        #endregion

        public static void m(object msg) { System.Diagnostics.Debug.WriteLine(msg == null ? "null" : msg.ToString()); }

    }
}
