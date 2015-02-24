namespace CSMD
{
    partial class SettingsF
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.components = new System.ComponentModel.Container();
        	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsF));
        	this.librariesLB = new System.Windows.Forms.ListBox();
        	this.librariesCMS = new System.Windows.Forms.ContextMenuStrip(this.components);
        	this.addTSMI = new System.Windows.Forms.ToolStripMenuItem();
        	this.deleteTSMI = new System.Windows.Forms.ToolStripMenuItem();
        	this.tss = new System.Windows.Forms.ToolStripSeparator();
        	this.restoreTSMI = new System.Windows.Forms.ToolStripMenuItem();
        	this.label1 = new System.Windows.Forms.Label();
        	this.saveExitB = new System.Windows.Forms.Button();
        	this.netL = new System.Windows.Forms.Label();
        	this.netCB = new System.Windows.Forms.ComboBox();
        	this.deleteCB = new System.Windows.Forms.CheckBox();
        	this.applyB = new System.Windows.Forms.Button();
        	this.cancelB = new System.Windows.Forms.Button();
        	this.executableTB = new System.Windows.Forms.TextBox();
        	this.searchB = new System.Windows.Forms.Button();
        	this.executableL = new System.Windows.Forms.Label();
        	this.executableSFD = new System.Windows.Forms.SaveFileDialog();
        	this.reassociateB = new System.Windows.Forms.Button();
        	this.randomCB = new System.Windows.Forms.CheckBox();
        	this.addB = new System.Windows.Forms.Button();
        	this.deleteB = new System.Windows.Forms.Button();
        	this.librariesTT = new System.Windows.Forms.ToolTip(this.components);
        	this.librariesOFD = new System.Windows.Forms.OpenFileDialog();
        	this.autocompletionCB = new System.Windows.Forms.CheckBox();
        	this.importsOnTheFlyCB = new System.Windows.Forms.CheckBox();
        	this.librariesCMS.SuspendLayout();
        	this.SuspendLayout();
        	// 
        	// librariesLB
        	// 
        	this.librariesLB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left)));
        	this.librariesLB.ContextMenuStrip = this.librariesCMS;
        	this.librariesLB.FormattingEnabled = true;
        	this.librariesLB.Location = new System.Drawing.Point(12, 25);
        	this.librariesLB.Name = "librariesLB";
        	this.librariesLB.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
        	this.librariesLB.Size = new System.Drawing.Size(165, 186);
        	this.librariesLB.TabIndex = 0;
        	this.librariesLB.SelectedIndexChanged += new System.EventHandler(this.librariesLB_SelectedIndexChanged);
        	this.librariesLB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.librariesLB_KeyDown);
        	this.librariesLB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.librariesLB_MouseMove);
        	// 
        	// librariesCMS
        	// 
        	this.librariesCMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.addTSMI,
			this.deleteTSMI,
			this.tss,
			this.restoreTSMI});
        	this.librariesCMS.Name = "librariesCMS";
        	this.librariesCMS.Size = new System.Drawing.Size(198, 76);
        	// 
        	// addTSMI
        	// 
        	this.addTSMI.Name = "addTSMI";
        	this.addTSMI.Size = new System.Drawing.Size(197, 22);
        	this.addTSMI.Text = "Add libraries";
        	this.addTSMI.Click += new System.EventHandler(this.add_Click);
        	// 
        	// deleteTSMI
        	// 
        	this.deleteTSMI.Name = "deleteTSMI";
        	this.deleteTSMI.Size = new System.Drawing.Size(197, 22);
        	this.deleteTSMI.Text = "Delete libraries";
        	this.deleteTSMI.Click += new System.EventHandler(this.delete_Click);
        	// 
        	// tss
        	// 
        	this.tss.Name = "tss";
        	this.tss.Size = new System.Drawing.Size(194, 6);
        	// 
        	// restoreTSMI
        	// 
        	this.restoreTSMI.Name = "restoreTSMI";
        	this.restoreTSMI.Size = new System.Drawing.Size(197, 22);
        	this.restoreTSMI.Text = "Restore default libraries";
        	this.restoreTSMI.Click += new System.EventHandler(this.restoreTSMI_Click);
        	// 
        	// label1
        	// 
        	this.label1.AutoSize = true;
        	this.label1.Location = new System.Drawing.Point(9, 9);
        	this.label1.Name = "label1";
        	this.label1.Size = new System.Drawing.Size(101, 13);
        	this.label1.TabIndex = 1;
        	this.label1.Text = "Referenced libaries:";
        	// 
        	// saveExitB
        	// 
        	this.saveExitB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        	this.saveExitB.Location = new System.Drawing.Point(282, 276);
        	this.saveExitB.Name = "saveExitB";
        	this.saveExitB.Size = new System.Drawing.Size(90, 23);
        	this.saveExitB.TabIndex = 4;
        	this.saveExitB.Text = "Save and exit";
        	this.saveExitB.UseVisualStyleBackColor = true;
        	this.saveExitB.Click += new System.EventHandler(this.saveExitB_Click);
        	// 
        	// netL
        	// 
        	this.netL.AutoSize = true;
        	this.netL.Location = new System.Drawing.Point(183, 25);
        	this.netL.Name = "netL";
        	this.netL.Size = new System.Drawing.Size(127, 13);
        	this.netL.TabIndex = 5;
        	this.netL.Text = ".NET Framework version:";
        	// 
        	// netCB
        	// 
        	this.netCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        	this.netCB.FormattingEnabled = true;
        	this.netCB.Location = new System.Drawing.Point(316, 22);
        	this.netCB.Name = "netCB";
        	this.netCB.Size = new System.Drawing.Size(62, 21);
        	this.netCB.TabIndex = 6;
        	// 
        	// deleteCB
        	// 
        	this.deleteCB.AutoSize = true;
        	this.deleteCB.Checked = true;
        	this.deleteCB.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.deleteCB.Location = new System.Drawing.Point(183, 155);
        	this.deleteCB.Name = "deleteCB";
        	this.deleteCB.Size = new System.Drawing.Size(197, 17);
        	this.deleteCB.TabIndex = 7;
        	this.deleteCB.Text = "Delete generated executable on exit";
        	this.deleteCB.UseVisualStyleBackColor = true;
        	// 
        	// applyB
        	// 
        	this.applyB.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
        	this.applyB.Location = new System.Drawing.Point(138, 276);
        	this.applyB.Name = "applyB";
        	this.applyB.Size = new System.Drawing.Size(90, 23);
        	this.applyB.TabIndex = 8;
        	this.applyB.Text = "Apply changes";
        	this.applyB.UseVisualStyleBackColor = true;
        	this.applyB.Click += new System.EventHandler(this.applyB_Click);
        	// 
        	// cancelB
        	// 
        	this.cancelB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        	this.cancelB.Location = new System.Drawing.Point(12, 276);
        	this.cancelB.Name = "cancelB";
        	this.cancelB.Size = new System.Drawing.Size(62, 23);
        	this.cancelB.TabIndex = 9;
        	this.cancelB.Text = "Cancel";
        	this.cancelB.UseVisualStyleBackColor = true;
        	this.cancelB.Click += new System.EventHandler(this.cancelB_Click);
        	// 
        	// executableTB
        	// 
        	this.executableTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
        	this.executableTB.Location = new System.Drawing.Point(183, 78);
        	this.executableTB.Name = "executableTB";
        	this.executableTB.Size = new System.Drawing.Size(156, 20);
        	this.executableTB.TabIndex = 10;
        	this.executableTB.TextChanged += new System.EventHandler(this.ExecutableTBTextChanged);
        	// 
        	// searchB
        	// 
        	this.searchB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        	this.searchB.Location = new System.Drawing.Point(345, 78);
        	this.searchB.Name = "searchB";
        	this.searchB.Size = new System.Drawing.Size(27, 23);
        	this.searchB.TabIndex = 11;
        	this.searchB.Text = "...";
        	this.searchB.UseVisualStyleBackColor = true;
        	this.searchB.Click += new System.EventHandler(this.searchB_Click);
        	// 
        	// executableL
        	// 
        	this.executableL.AutoSize = true;
        	this.executableL.Location = new System.Drawing.Point(183, 64);
        	this.executableL.Name = "executableL";
        	this.executableL.Size = new System.Drawing.Size(87, 13);
        	this.executableL.TabIndex = 12;
        	this.executableL.Text = "Executable path:";
        	// 
        	// executableSFD
        	// 
        	this.executableSFD.FileName = "csmd.exe";
        	this.executableSFD.Filter = "Executable file|*.exe";
        	this.executableSFD.OverwritePrompt = false;
        	// 
        	// reassociateB
        	// 
        	this.reassociateB.Location = new System.Drawing.Point(257, 178);
        	this.reassociateB.Name = "reassociateB";
        	this.reassociateB.Size = new System.Drawing.Size(115, 23);
        	this.reassociateB.TabIndex = 13;
        	this.reassociateB.Text = "Reassociate file type";
        	this.reassociateB.UseVisualStyleBackColor = true;
        	this.reassociateB.Click += new System.EventHandler(this.reassociateB_Click);
        	// 
        	// randomCB
        	// 
        	this.randomCB.AutoSize = true;
        	this.randomCB.Location = new System.Drawing.Point(183, 104);
        	this.randomCB.Name = "randomCB";
        	this.randomCB.Size = new System.Drawing.Size(147, 30);
        	this.randomCB.TabIndex = 14;
        	this.randomCB.Text = "Use random filename\r\n(allows multiple instances)";
        	this.randomCB.UseVisualStyleBackColor = true;
        	this.randomCB.CheckedChanged += new System.EventHandler(this.randomCB_CheckedChanged);
        	// 
        	// addB
        	// 
        	this.addB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        	this.addB.Location = new System.Drawing.Point(105, 228);
        	this.addB.Name = "addB";
        	this.addB.Size = new System.Drawing.Size(72, 23);
        	this.addB.TabIndex = 15;
        	this.addB.Text = "Add libraries";
        	this.addB.UseVisualStyleBackColor = true;
        	this.addB.Click += new System.EventHandler(this.add_Click);
        	// 
        	// deleteB
        	// 
        	this.deleteB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        	this.deleteB.Location = new System.Drawing.Point(12, 228);
        	this.deleteB.Name = "deleteB";
        	this.deleteB.Size = new System.Drawing.Size(49, 23);
        	this.deleteB.TabIndex = 16;
        	this.deleteB.Text = "Delete";
        	this.deleteB.UseVisualStyleBackColor = true;
        	this.deleteB.Click += new System.EventHandler(this.delete_Click);
        	// 
        	// librariesTT
        	// 
        	this.librariesTT.AutoPopDelay = 5000;
        	this.librariesTT.InitialDelay = 0;
        	this.librariesTT.ReshowDelay = 100;
        	this.librariesTT.ShowAlways = true;
        	// 
        	// librariesOFD
        	// 
        	this.librariesOFD.Filter = "Dynamic libraries|*.dll";
        	this.librariesOFD.Multiselect = true;
        	// 
        	// autocompletionCB
        	// 
        	this.autocompletionCB.AutoSize = true;
        	this.autocompletionCB.Checked = true;
        	this.autocompletionCB.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.autocompletionCB.Location = new System.Drawing.Point(183, 207);
        	this.autocompletionCB.Name = "autocompletionCB";
        	this.autocompletionCB.Size = new System.Drawing.Size(140, 17);
        	this.autocompletionCB.TabIndex = 17;
        	this.autocompletionCB.Text = "Enable autocompletion?";
        	this.autocompletionCB.UseVisualStyleBackColor = true;
        	this.autocompletionCB.CheckedChanged += new System.EventHandler(this.AutocompletionCBCheckedChanged);
        	// 
        	// importsOnTheFlyCB
        	// 
        	this.importsOnTheFlyCB.AutoSize = true;
        	this.importsOnTheFlyCB.Checked = true;
        	this.importsOnTheFlyCB.CheckState = System.Windows.Forms.CheckState.Checked;
        	this.importsOnTheFlyCB.Location = new System.Drawing.Point(183, 230);
        	this.importsOnTheFlyCB.Name = "importsOnTheFlyCB";
        	this.importsOnTheFlyCB.Size = new System.Drawing.Size(140, 17);
        	this.importsOnTheFlyCB.TabIndex = 18;
        	this.importsOnTheFlyCB.Text = "Import usings on the fly?";
        	this.importsOnTheFlyCB.UseVisualStyleBackColor = true;
        	// 
        	// SettingsF
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(384, 311);
        	this.Controls.Add(this.importsOnTheFlyCB);
        	this.Controls.Add(this.autocompletionCB);
        	this.Controls.Add(this.deleteB);
        	this.Controls.Add(this.addB);
        	this.Controls.Add(this.randomCB);
        	this.Controls.Add(this.reassociateB);
        	this.Controls.Add(this.executableL);
        	this.Controls.Add(this.searchB);
        	this.Controls.Add(this.executableTB);
        	this.Controls.Add(this.cancelB);
        	this.Controls.Add(this.applyB);
        	this.Controls.Add(this.deleteCB);
        	this.Controls.Add(this.netCB);
        	this.Controls.Add(this.netL);
        	this.Controls.Add(this.saveExitB);
        	this.Controls.Add(this.label1);
        	this.Controls.Add(this.librariesLB);
        	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        	this.MinimumSize = new System.Drawing.Size(400, 350);
        	this.Name = "SettingsF";
        	this.Text = "Settings";
        	this.librariesCMS.ResumeLayout(false);
        	this.ResumeLayout(false);
        	this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox librariesLB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveExitB;
        private System.Windows.Forms.Label netL;
        private System.Windows.Forms.ComboBox netCB;
        private System.Windows.Forms.CheckBox deleteCB;
        private System.Windows.Forms.Button applyB;
        private System.Windows.Forms.Button cancelB;
        private System.Windows.Forms.TextBox executableTB;
        private System.Windows.Forms.Button searchB;
        private System.Windows.Forms.Label executableL;
        private System.Windows.Forms.SaveFileDialog executableSFD;
        private System.Windows.Forms.Button reassociateB;
        private System.Windows.Forms.CheckBox randomCB;
        private System.Windows.Forms.Button addB;
        private System.Windows.Forms.Button deleteB;
        private System.Windows.Forms.ToolTip librariesTT;
        private System.Windows.Forms.ContextMenuStrip librariesCMS;
        private System.Windows.Forms.ToolStripMenuItem addTSMI;
        private System.Windows.Forms.ToolStripMenuItem deleteTSMI;
        private System.Windows.Forms.ToolStripSeparator tss;
        private System.Windows.Forms.ToolStripMenuItem restoreTSMI;
        private System.Windows.Forms.OpenFileDialog librariesOFD;
        private System.Windows.Forms.CheckBox autocompletionCB;
        private System.Windows.Forms.CheckBox importsOnTheFlyCB;
    }
}