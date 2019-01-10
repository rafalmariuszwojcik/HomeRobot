namespace RobotControl.Windows.Views
{
  public partial class OutputView : BaseView
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