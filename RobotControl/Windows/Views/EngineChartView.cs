using RobotControl.Command;
using RobotControl.Core;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Windows.Views
{
  public partial class EngineChartView : RobotControl.Windows.Views.BaseView, IListener<CommandPackage>
  {
    public EngineChartView()
    {
      InitializeComponent();
    }

    void IListener<CommandPackage>.DataReceived(Communication.IChannel channel, IEnumerable<CommandPackage> data)
    {
      chart1.Series.SuspendUpdates();



      foreach (var x in data)
      {
        var lastItem = x;
        if (lastItem?.Command is EngineSpeedCommand)
        {
          if (this.chart1.Series["Speed"].Points.Count() > 100)
          {
            this.chart1.Series["Speed"].Points.RemoveAt(0);
          }

          if (this.chart1.Series["PWM"].Points.Count() > 100)
          {
            this.chart1.Series["PWM"].Points.RemoveAt(0);
          }

          if (this.chart1.Series["AvgSpeed"].Points.Count() > 100)
          {
            this.chart1.Series["AvgSpeed"].Points.RemoveAt(0);
          }

          this.chart1.Series["Speed"].Points.AddXY(((EngineSpeedCommand)lastItem.Command).Distance, ((EngineSpeedCommand)lastItem.Command).Speed);
          this.chart1.Series["AvgSpeed"].Points.AddXY(((EngineSpeedCommand)lastItem.Command).Distance, ((EngineSpeedCommand)lastItem.Command).AvgSpeed);
          this.chart1.Series["PWM"].Points.AddXY(((EngineSpeedCommand)lastItem.Command).Distance, ((EngineSpeedCommand)lastItem.Command).PWM);
        }
      }


      chart1.Series.ResumeUpdates();


    }
  }
}
