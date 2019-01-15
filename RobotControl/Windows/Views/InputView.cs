using RobotControl.Communication;
using System.Linq;

namespace RobotControl.Windows.Views
{
  public partial class InputView : BaseView
  {
    public InputView()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, System.EventArgs e)
    {
      var port = CommunicationManager.Instance.Items.Where(x => x.Active).FirstOrDefault();
      if (port != null)
      {
        port.Send(uxInputText.Text);
        uxInputText.Clear();
      }
    }
  }
}
