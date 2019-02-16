using RobotControl.Communication;
using RobotControl.Windows.Forms;
using RobotControl.Messages;
using RobotControl.Windows.Views;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using System.Threading;
using System;

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

    private void toolStripButton2_Click(object sender, System.EventArgs e)
    {
      MessageManager.Instance.DataReceived(this, new[] { $"Bolek i Lolek.;E00,0,{i++},{i++},{i++};DIST,1,2,3,4;" });
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
  }
}
