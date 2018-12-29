using System;

namespace RobotControl.Views
{
  public partial class OutputView : RobotControl.Forms.BaseView
  {
    public OutputView()
    {
      InitializeComponent();
    }

    protected override void MessageReceived(object sender, string message)
    {
      uxOutputText.AppendText(message);
    }
  }
}