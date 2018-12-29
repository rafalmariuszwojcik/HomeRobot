using System;

namespace RobotControl.Views
{
  public partial class OutputView : BaseView
  {
    public OutputView() : base(true, false)
    {
      InitializeComponent();
    }

    protected override void MessageReceived(object sender, string message)
    {
      uxOutputText.AppendText(message);
    }
  }
}