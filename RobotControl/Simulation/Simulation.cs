using RobotControl.Command;
using RobotControl.Communication;
using RobotControl.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Simulation
{
  public class Simulation : SimulationItem, ISimulation, ICommandListener
  {
    public ISimulationArea SimulationArea => new SimulationArea(new Length(10, MeasurementUnit.Meter), new Length(1, MeasurementUnit.Meter));
    

    

    

    

    protected override void Dispose(bool disposing)
    {
      ;// throw new NotImplementedException();
    }

    void IListener<ICommand>.DataReceived(IChannel channel, IEnumerable<ICommand> data)
    {
      foreach (var item in Items)
      {
        item.ResetState();
      }

      foreach (var item in Items)
      {
        (item as ICommandListener)?.DataReceived(channel, data);
      }

      //StateChanged = Items.Any(x => x.State);
    }
  }
}
