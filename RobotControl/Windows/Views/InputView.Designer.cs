namespace RobotControl.Windows.Views
{
  partial class InputView
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
      this.uxInputText = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // uxInputText
      // 
      this.uxInputText.BackColor = System.Drawing.SystemColors.Window;
      this.uxInputText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.uxInputText.Location = new System.Drawing.Point(8, 8);
      this.uxInputText.Multiline = true;
      this.uxInputText.Name = "uxInputText";
      this.uxInputText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.uxInputText.Size = new System.Drawing.Size(196, 68);
      this.uxInputText.TabIndex = 1;
      this.uxInputText.WordWrap = false;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(12, 82);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "button1";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // InputView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.uxInputText);
      this.Name = "InputView";
      this.Text = "InputView";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox uxInputText;
    private System.Windows.Forms.Button button1;
  }
}