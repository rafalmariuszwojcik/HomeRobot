using RobotControl.Communication;
using RobotControl.Forms;
using RobotControl.Messages;
using RobotControl.Views;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RobotControl
{
  public partial class formRobotControl : BaseForm
  {
    private PropertyGrid OptionsPropertyGrid;
   // private DockPanel s;



    public formRobotControl()
    {
      InitializeComponent();
      OptionsPropertyGrid = new PropertyGrid();
      OptionsPropertyGrid.Size = new Size(175, 250);

      this.Controls.Add(OptionsPropertyGrid);
      StatusBar statusBar1 = new StatusBar();
      this.Controls.Add(statusBar1);

      //dockPanel1
      var dockContent = new SimulationView();
      var dockContent2 = new SimulationView();

      dockContent.Show(this.dockPanel1, DockState.Document);
      dockContent2.Show(this.dockPanel1, DockState.Document);

    }

    private void button1_Click(object sender, System.EventArgs e)
    {
      /*
      using (var port = new Serial(new SerialConfiguration { Port = "COM3", BaudRate = 9600 }))
      {
        port.Open();
      }
      */
      //OptionsPropertyGrid.SelectedObject = new SerialConfiguration { Port = "COM3", BaudRate = 9600 };

     // OptionsPropertyGrid.SelectedObject = areaViewControl1;

      //OptionsPropertyGrid.PropertySort = PropertySort.NoSort;
      //OptionsPropertyGrid.ToolbarVisible = false;
      //OptionsPropertyGrid.LineColor = SystemColors.ButtonFace;
      //OptionsPropertyGrid.


    }

    private void trackBar1_ValueChanged(object sender, System.EventArgs e)
    {
      //this.areaViewControl1.Zoom = trackBar1.Value;
    }

    private void testsToolStripMenuItem_Click(object sender, System.EventArgs e)
    {

    }

    private void toolStripButton1_Click(object sender, System.EventArgs e)
    {
      var dockContent = new CommunicationManagerView();
      dockContent.Show(this.dockPanel1, DockState.Float);
      var dockOutput = new OutputView();
      dockOutput.Show(this.dockPanel1, DockState.Float);
    }

    private void toolStripButton2_Click(object sender, System.EventArgs e)
    {
      MessageManager.Instance.MessageReceived(this, "Bolek i Lolek.");
    }
  }
}
