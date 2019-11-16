using RobotControl.Command;
using RobotControl.Messages;
using RobotControl.Windows.Forms;
using RobotControl.Windows.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RobotControl
{
  public partial class formRobotControl : BaseForm
  {
    private PropertyGrid OptionsPropertyGrid;
    // private DockPanel s;

    private XInputController controller = new XInputController();



    public formRobotControl()
    {
      InitializeComponent();







      OptionsPropertyGrid = new PropertyGrid();
      OptionsPropertyGrid.Size = new Size(175, 250);

      this.Controls.Add(OptionsPropertyGrid);
      StatusBar statusBar1 = new StatusBar();
      this.Controls.Add(statusBar1);

      //dockPanel1
      ///var dockContent = new SimulationView();
      //var dockContent2 = new SimulationView();

      //dockContent.Show(this.dockPanel1, DockState.Document);
      //dockContent2.Show(this.dockPanel1, DockState.Document);

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

    }

    int i = 0;
    int distanceL = 0;
    int distanceR = 0;

    private void toolStripButton2_Click(object sender, System.EventArgs e)
    {
      //MessageManager.Instance.DataReceived(this, new[] { $"Bolek i Lolek.;E00,0,{i++},{i++},{i++};DIST,2,{distanceL},2,{distanceR};" });
      //MessageManager.Instance.DataReceived(this, new[] { $"ENC,0,{distanceL},0,2;" });

      /*
      if (distanceR > distanceL)
      {
        distanceL += 1;
      }
      else
      {
        distanceR += 1;
      }
      */

      //var x = MessageManager.Instance;
      //var y = x;
      Task.Factory.StartNew(new Action(() =>
      {
        var list = new List<long>();
        for (var i = 0; i < 90; i++)
        {
          var totalMilliseconds = (long)new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
          MessageManager.Instance.DataReceived(this, new[] { $"ENC,1,{i},{totalMilliseconds},2;{Environment.NewLine}" });

          Thread.Sleep(30);

          totalMilliseconds = (long)new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
          MessageManager.Instance.DataReceived(this, new[] { $"ENC,0,{i},{totalMilliseconds},2;{Environment.NewLine}" });

          list.Add(totalMilliseconds);

          // Dont use Task.Delay, it is very not precise.
          //Thread.Sleep(25);
          Thread.Sleep(20);
        }

        for (var i = 0; i < list.Count() - 1; i++)
        {
          list[i] = list[i + 1] - list[i];
        }

        var xyz = list;
      }));

      /*
      Task.Factory.StartNew(new Action(() =>
      {
        var list = new List<long>();
        for (var i = 0; i < 49; i++)
        {
          var totalMilliseconds = (long)new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
          //MessageManager.Instance.DataReceived(this, new[] { $"ENC,0,{i},{totalMilliseconds},2;{Environment.NewLine}" });
          MessageManager.Instance.DataReceived(this, new[] { $"ENC,1,{i},{totalMilliseconds},2;{Environment.NewLine}" });
          list.Add(totalMilliseconds);

          // Dont use Task.Delay, it is very not precise.
          Thread.Sleep(40);
        }

        for (var i = 0; i < list.Count() - 1; i++)
        {
          list[i] = list[i + 1] - list[i];
        }

        var xyz = list;
      }));
      */
    }

    private void toolStripButton3_Click(object sender, System.EventArgs e)
    {
      var dockOutput = new OutputView();
      dockOutput.Show(this.dockPanel1, DockState.Float);
    }

    private void toolStripButton5_Click(object sender, System.EventArgs e)
    {
      var dockOutput = new EngineInfoView();
      dockOutput.Show(this.dockPanel1, DockState.Float);
    }

    private void formRobotControl_FormClosing(object sender, FormClosingEventArgs e)
    {
      dockPanel1.SaveAsXml(@"c:\temp\layout.xml");
    }

    private void formRobotControl_Load(object sender, System.EventArgs e)
    {
      var m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
      if (File.Exists(@"c:\temp\layout.xml"))
      {
        dockPanel1.LoadFromXml(@"c:\temp\layout.xml", m_deserializeDockContent);
      }

      // backgroundWorker1.RunWorkerAsync();
    }

    private IDockContent GetContentFromPersistString(string persistString)
    {
      if (persistString == typeof(SimulationView).ToString())
      {
        return new SimulationView();
      }
      else if (persistString == typeof(EngineInfoView).ToString())
      {
        return new EngineInfoView();
      }
      else if (persistString == typeof(OutputView).ToString())
      {
        return new OutputView();
      }
      else if (persistString == typeof(CommunicationManagerView).ToString())
      {
        return new CommunicationManagerView();
      }
      else if (persistString == typeof(InputView).ToString())
      {
        return new InputView();
      }
      else if (persistString == typeof(EngineChartView).ToString())
      {
        return new EngineChartView();
      }
      else
      {
        return null;
      }

      /*
      if (persistString == typeof(DummySolutionExplorer).ToString())
        return m_solutionExplorer;
      else if (persistString == typeof(DummyPropertyWindow).ToString())
        return m_propertyWindow;
      else if (persistString == typeof(DummyToolbox).ToString())
        return m_toolbox;
      else if (persistString == typeof(DummyOutputWindow).ToString())
        return m_outputWindow;
      else if (persistString == typeof(DummyTaskList).ToString())
        return m_taskList;
      else
      {
        // DummyDoc overrides GetPersistString to add extra information into persistString.
        // Any DockContent may override this value to add any needed information for deserialization.

        string[] parsedStrings = persistString.Split(new char[] { ',' });
        if (parsedStrings.Length != 3)
          return null;

        if (parsedStrings[0] != typeof(DummyDoc).ToString())
          return null;

        DummyDoc dummyDoc = new DummyDoc();
        if (parsedStrings[1] != string.Empty)
          dummyDoc.FileName = parsedStrings[1];
        if (parsedStrings[2] != string.Empty)
          dummyDoc.Text = parsedStrings[2];

        return dummyDoc;
        
      return null;
      }*/
      return null;
    }

    private void toolStripButton4_Click(object sender, System.EventArgs e)
    {
      var dockOutput = new SimulationView();
      dockOutput.Show(this.dockPanel1, DockState.Document);
    }

    private void toolStripButton6_Click(object sender, System.EventArgs e)
    {
      var dockOutput = new InputView();
      dockOutput.Show(this.dockPanel1, DockState.Document);
    }

    int cnt = 0;

    private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      while (true)
      {
        Thread.Sleep(3);
        MessageManager.Instance.DataReceived(null, new[] { $"Bolek i Lolek {cnt++}.{Environment.NewLine}" });
      }
    }

    private void toolStripButton7_Click(object sender, EventArgs e)
    {
      var dockOutput = new EngineChartView();
      dockOutput.Show(this.dockPanel1, DockState.Document);
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      //return;
      if (!controller.Update()) { return; }
      //this.Text = controller.leftThumb.X.ToString();
      var cmd = new List<ICommand>();
      cmd.Add(new ControllerCommand { X = controller.leftThumb.X, Y = controller.leftThumb.Y, RX = controller.rightThumb.X });
      CommandManager.Instance.DataReceived(this, cmd);
    }
  }
}
