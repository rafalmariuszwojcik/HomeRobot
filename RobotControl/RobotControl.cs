using RobotControl.Communication;
using System.Windows.Forms;

namespace RobotControl
{
  public partial class formRobotControl : Form
  {
    public formRobotControl()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, System.EventArgs e)
    {
      using (var port = new Serial(new SerialConfiguration { Port = "COM3", BaudRate = 9600 }))
      {
        port.Open();
      }
    }
  }
}
