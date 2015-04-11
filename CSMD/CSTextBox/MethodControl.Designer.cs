
partial class MethodControl
{
	/// <summary>
	/// Designer variable used to keep track of non-visual components.
	/// </summary>
	private System.ComponentModel.IContainer components = null;
	private System.Windows.Forms.Label countL;
	private System.Windows.Forms.WebBrowser methodWB;
	
	/// <summary>
	/// Disposes resources used by the control.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing) {
			if (components != null) {
				components.Dispose();
			}
		}
		base.Dispose(disposing);
	}
	
	/// <summary>
	/// This method is required for Windows Forms designer support.
	/// Do not change the method contents inside the source code editor. The Forms designer might
	/// not be able to load this method if it was changed manually.
	/// </summary>
	private void InitializeComponent()
	{
		this.countL = new System.Windows.Forms.Label();
		this.methodWB = new System.Windows.Forms.WebBrowser();
		this.SuspendLayout();
		// 
		// countL
		// 
		this.countL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.countL.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
		this.countL.Location = new System.Drawing.Point(12, 38);
		this.countL.Name = "countL";
		this.countL.Size = new System.Drawing.Size(44, 21);
		this.countL.TabIndex = 0;
		this.countL.Text = "0/0";
		// 
		// methodWB
		// 
		this.methodWB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
		this.methodWB.Location = new System.Drawing.Point(0, 0);
		this.methodWB.MinimumSize = new System.Drawing.Size(20, 20);
		this.methodWB.Name = "methodWB";
		this.methodWB.ScrollBarsEnabled = false;
		this.methodWB.Size = new System.Drawing.Size(510, 44);
		this.methodWB.TabIndex = 1;
		// 
		// MethodControl
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.Controls.Add(this.countL);
		this.Controls.Add(this.methodWB);
		this.Name = "MethodControl";
		this.Size = new System.Drawing.Size(510, 59);
		this.ResumeLayout(false);

	}
}
