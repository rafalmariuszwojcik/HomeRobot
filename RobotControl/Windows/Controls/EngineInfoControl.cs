using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RobotControl.Command;

namespace RobotControl.Windows.Controls
{
  public partial class EngineInfoControl : RobotControl.Windows.Controls.BaseControl
  {
    private EngineInfo engineInfo = new EngineInfo();
    private readonly PropertyGrid propertyGrid = new PropertyGrid();

    public EngineInfoControl()
    {
      InitializeComponent();
      propertyGrid.Dock = DockStyle.Fill;
      propertyGrid.SelectedObject = engineInfo;
      Controls.Add(propertyGrid);
    }

    protected override void CommandReceived(object sender, ICommand message)
    {
      if (message is EngineSpeedCommand)
      {
        engineInfo.Speed = ((EngineSpeedCommand)message).Speed;
        propertyGrid.Refresh();
      }
    }

    private class EngineInfo
    {
      public int Speed { get; set; }
    }
  }
}
