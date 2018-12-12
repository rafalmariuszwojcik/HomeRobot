namespace RobotControl
{
  partial class formRobotControl
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formRobotControl));
      this.button1 = new System.Windows.Forms.Button();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
      this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
      this.printDocument1 = new System.Drawing.Printing.PrintDocument();
      this.trackBar1 = new System.Windows.Forms.TrackBar();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ssssToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.areaViewControl1 = new RobotControl.Controls.AreaViewControl();
      this.visualStudioToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
      this.toolStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // button1
      // 
      resources.ApplyResources(this.button1, "button1");
      this.button1.Name = "button1";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton3});
      resources.ApplyResources(this.toolStrip1, "toolStrip1");
      this.toolStrip1.Name = "toolStrip1";
      // 
      // toolStripButton1
      // 
      this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
      this.toolStripButton1.Name = "toolStripButton1";
      // 
      // toolStripButton2
      // 
      this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
      this.toolStripButton2.Name = "toolStripButton2";
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
      // 
      // toolStripButton3
      // 
      this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      resources.ApplyResources(this.toolStripButton3, "toolStripButton3");
      this.toolStripButton3.Name = "toolStripButton3";
      // 
      // trackBar1
      // 
      resources.ApplyResources(this.trackBar1, "trackBar1");
      this.trackBar1.Maximum = 100;
      this.trackBar1.Minimum = 1;
      this.trackBar1.Name = "trackBar1";
      this.trackBar1.Value = 1;
      this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testsToolStripMenuItem});
      resources.ApplyResources(this.menuStrip1, "menuStrip1");
      this.menuStrip1.Name = "menuStrip1";
      // 
      // testsToolStripMenuItem
      // 
      this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssssToolStripMenuItem});
      this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
      resources.ApplyResources(this.testsToolStripMenuItem, "testsToolStripMenuItem");
      this.testsToolStripMenuItem.Click += new System.EventHandler(this.testsToolStripMenuItem_Click);
      // 
      // ssssToolStripMenuItem
      // 
      this.ssssToolStripMenuItem.Name = "ssssToolStripMenuItem";
      resources.ApplyResources(this.ssssToolStripMenuItem, "ssssToolStripMenuItem");
      // 
      // areaViewControl1
      // 
      resources.ApplyResources(this.areaViewControl1, "areaViewControl1");
      this.areaViewControl1.Name = "areaViewControl1";
      this.areaViewControl1.Zoom = 100;
      // 
      // visualStudioToolStripExtender1
      // 
      this.visualStudioToolStripExtender1.DefaultRenderer = null;
      // 
      // formRobotControl
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.trackBar1);
      this.Controls.Add(this.areaViewControl1);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.menuStrip1);
      this.Controls.Add(this.button1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "formRobotControl";
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButton1;
    private System.Windows.Forms.ToolStripButton toolStripButton2;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripButton3;
    private System.Drawing.Printing.PrintDocument printDocument1;
    private Controls.AreaViewControl areaViewControl1;
    private System.Windows.Forms.TrackBar trackBar1;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem testsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem ssssToolStripMenuItem;
    private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender visualStudioToolStripExtender1;
  }
}

