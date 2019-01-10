namespace RobotControl.Windows.Views
{
  partial class EngineInfoView
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
      this.engineInfoControl1 = new RobotControl.Windows.Controls.EngineInfoControl();
      this.SuspendLayout();
      // 
      // engineInfoControl1
      // 
      this.engineInfoControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.engineInfoControl1.Location = new System.Drawing.Point(0, 0);
      this.engineInfoControl1.Name = "engineInfoControl1";
      this.engineInfoControl1.Size = new System.Drawing.Size(196, 218);
      this.engineInfoControl1.TabIndex = 0;
      // 
      // EngineInfoView
      // 
      this.ClientSize = new System.Drawing.Size(196, 218);
      this.Controls.Add(this.engineInfoControl1);
      this.Name = "EngineInfoView";
      this.ResumeLayout(false);

    }

    #endregion

    private Controls.EngineInfoControl engineInfoControl1;
  }
}
