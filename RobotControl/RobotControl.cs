using RobotControl.Communication;
using RobotControl.Forms;
using System.Drawing;
using System.Windows.Forms;

namespace RobotControl
{
  public partial class formRobotControl : Form
  {
    private PropertyGrid OptionsPropertyGrid;
   // private DockPanel s;



    public formRobotControl()
    {
      InitializeComponent();
      OptionsPropertyGrid = new PropertyGrid();
      OptionsPropertyGrid.Size = new Size(175, 250);

      this.Controls.Add(OptionsPropertyGrid);
      new SimulationView().Show();
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

      OptionsPropertyGrid.SelectedObject = areaViewControl1;

      //OptionsPropertyGrid.PropertySort = PropertySort.NoSort;
      //OptionsPropertyGrid.ToolbarVisible = false;
      //OptionsPropertyGrid.LineColor = SystemColors.ButtonFace;
      //OptionsPropertyGrid.


    }

    private void trackBar1_ValueChanged(object sender, System.EventArgs e)
    {
      this.areaViewControl1.Zoom = trackBar1.Value;
    }

    private void testsToolStripMenuItem_Click(object sender, System.EventArgs e)
    {

    }
  }
}
