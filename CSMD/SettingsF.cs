using CSMD.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CSMD
{
    public partial class SettingsF : Form
    {
        string NetFrameworkFolder;

        string[] DefaultLibraries = new string[] {
            "System.dll",
            "System.Core.dll",
            "System.Data.dll",
            "System.Data.DataSetExtensions.dll",
            "System.Deployment.dll",
            "System.Drawing.dll",
            "System.Windows.Forms.dll",
            "System.Xml.dll",
            "System.Xml.Linq.dll"
        };

        #region Setup

        public SettingsF()
        {
            InitializeComponent();

            librariesLB.Items.AddRange(Program.StringCollectionToArray(Settings.Default.ReferencedAssemblies));

            var netV = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
            foreach (var s in netV.GetSubKeyNames().Where(x => x.StartsWith("v")))
                netCB.Items.Add(s);
            netCB.SelectedItem = Settings.Default.NETVersion;

            executableTB.Text = Settings.Default.ExecutablePath;
            deleteCB.Checked = Settings.Default.DeleteOnExit;
            randomCB.Checked = Settings.Default.RandomName;

            executableSFD.InitialDirectory = Path.GetTempPath();

            NetFrameworkFolder = GetNFF();

            librariesOFD.InitialDirectory = NetFrameworkFolder;
        }

        #endregion

        string GetNFF() {
            string netFrameworkFolder = Environment.ExpandEnvironmentVariables(@"%windir%\Microsoft.NET\Framework");
            if (Directory.Exists(netFrameworkFolder + "64"))
                netFrameworkFolder += "64";
            string[] folders = Directory.GetDirectories(netFrameworkFolder);
            netFrameworkFolder = folders.Where(x => Path.GetFileName(x).StartsWith("v4.0")).ToArray()[0];
            return netFrameworkFolder + "\\";
        }

        private void reassociateB_Click(object sender, EventArgs e) {
            Cursor = Cursors.WaitCursor;
            Program.AssociateFileType("CSMD", "cst", "CSMD file", true, 1);
            Cursor = Cursors.Arrow;
            MessageBox.Show("File type reassociated successfully", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void searchB_Click(object sender, EventArgs e)
        {
            if (executableSFD.ShowDialog() == DialogResult.OK)
                executableTB.Text = executableSFD.FileName;
        }


        #region Apply and close

        private void saveExitB_Click(object sender, EventArgs e) {
            Apply();
            Close();
        }

        private void applyB_Click(object sender, EventArgs e)
        { Apply(); }

        void Apply() {
            Cursor = Cursors.WaitCursor;
            Settings.Default.NETVersion = (string)netCB.SelectedItem;

            StringCollection libraries = new StringCollection();
            foreach (var item in librariesLB.Items)
                libraries.Add((string)item);
            Settings.Default.ReferencedAssemblies = libraries;

            Settings.Default.RandomName = randomCB.Checked;
            Settings.Default.DeleteOnExit = deleteCB.Checked;

            Settings.Default.Save();

            MainF.c = new Compiler();
            Cursor = Cursors.Arrow;
        }

        private void cancelB_Click(object sender, EventArgs e)
        { Close(); }

        #endregion

        private void randomCB_CheckedChanged(object sender, EventArgs e) {
            deleteCB.Enabled = !randomCB.Checked;
        }

        #region Libraries

        private void librariesLB_SelectedIndexChanged(object sender, EventArgs e) {
            deleteB.Enabled = librariesLB.SelectedIndex > -1;
        }

        private void librariesLB_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete)
                Delete();
        }
        private void delete_Click(object sender, EventArgs e)
        { Delete(); }

        void Delete() {
            List<object> items = new List<object>();
            foreach (var item in librariesLB.SelectedItems)
                items.Add(item);

            foreach (var item in items)
                librariesLB.Items.Remove(item);
        }

        private void add_Click(object sender, EventArgs e) {
            Add();
        }

        void Add() {
            if (librariesOFD.ShowDialog() == DialogResult.OK)
                foreach (var filename in librariesOFD.FileNames)
                    librariesLB.Items.Add(filename.Replace(NetFrameworkFolder, ""));
        }

        private void restoreTSMI_Click(object sender, EventArgs e) {
            librariesLB.Items.Clear();
            librariesLB.Items.AddRange(DefaultLibraries);
        }


        private void librariesLB_MouseMove(object sender, MouseEventArgs e)
        {
            int i = librariesLB.IndexFromPoint(e.X, e.Y);
            if (i > -1)
            {
                string s = (string)librariesLB.Items[i];
                ListBox lb = (ListBox)sender;
                if (librariesTT.GetToolTip(lb) != s)
                    librariesTT.SetToolTip(lb, s);
            }
        }

        #endregion
    }
}
