using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;
using RobotControl.Command;

namespace RobotControl.Windows.Controls
{
  public partial class EngineInfoControl : RobotControl.Windows.Controls.BaseControl, IListenerControl<CommandPackage>
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

    void IListenerControl<CommandPackage>.MessageReceived(IChannel channel, IEnumerable<CommandPackage> data)
    {
      var lastItem = data.LastOrDefault();
      if (lastItem?.Command is EngineSpeedCommand)
      {
        engineInfo.Speed = ((EngineSpeedCommand)lastItem.Command).Speed;
        engineInfo.AvgSpeed = ((EngineSpeedCommand)lastItem.Command).AvgSpeed;
        engineInfo.PWM = ((EngineSpeedCommand)lastItem.Command).PWM;
        engineInfo.Distance = ((EngineSpeedCommand)lastItem.Command).Distance;
        propertyGrid.Refresh();
      }
    }

    private class EngineInfo
    {
      public int Speed { get; set; }
      public int AvgSpeed { get; set; }
      public int PWM { get; set; }
      public int Distance { get; set; }
    }
  }
}
