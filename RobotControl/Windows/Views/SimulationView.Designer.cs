namespace RobotControl.Windows.Views
{
  partial class SimulationView
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulationView));
      this.areaViewControl1 = new RobotControl.Windows.Controls.AreaViewControl();
      this.trackBar1 = new System.Windows.Forms.TrackBar();
      this.areaViewControl1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
      this.SuspendLayout();
      // 
      // areaViewControl1
      // 
      this.areaViewControl1.Controls.Add(this.trackBar1);
      resources.ApplyResources(this.areaViewControl1, "areaViewControl1");
      this.areaViewControl1.Name = "areaViewControl1";
      this.areaViewControl1.Zoom = 50;
      // 
      // trackBar1
      // 
      resources.ApplyResources(this.trackBar1, "trackBar1");
      this.trackBar1.Maximum = 25;
      this.trackBar1.Name = "trackBar1";
      // 
      // SimulationView
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.areaViewControl1);
      this.Name = "SimulationView";
      this.areaViewControl1.ResumeLayout(false);
      this.areaViewControl1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private Controls.AreaViewControl areaViewControl1;
    private System.Windows.Forms.TrackBar trackBar1;
  }
}