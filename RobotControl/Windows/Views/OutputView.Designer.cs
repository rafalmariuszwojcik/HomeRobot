namespace RobotControl.Windows.Views
{
  partial class OutputView
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
      this.uxOutputText = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // uxOutputText
      // 
      this.uxOutputText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.uxOutputText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.uxOutputText.Location = new System.Drawing.Point(0, 0);
      this.uxOutputText.Multiline = true;
      this.uxOutputText.Name = "uxOutputText";
      this.uxOutputText.ReadOnly = true;
      this.uxOutputText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.uxOutputText.Size = new System.Drawing.Size(893, 245);
      this.uxOutputText.TabIndex = 0;
      this.uxOutputText.WordWrap = false;
      // 
      // OutputView
      // 
      this.ClientSize = new System.Drawing.Size(893, 245);
      this.Controls.Add(this.uxOutputText);
      this.Name = "OutputView";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox uxOutputText;
  }
}
