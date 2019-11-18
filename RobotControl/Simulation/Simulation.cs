using RobotControl.Command;
using RobotControl.Command.Controller;
using RobotControl.Communication;
using RobotControl.Core;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Simulation
{
  public class Simulation : SimulationItem, ISimulation, ICommandListener
  {
    public ISimulationArea SimulationArea => new SimulationArea(new Length(10, MeasurementUnit.Meter), new Length(1, MeasurementUnit.Meter));

    void IListener<ICommand>.DataReceived(IChannel channel, IEnumerable<ICommand> data)
    {
      foreach (var item in Items)
      {
        item.ResetState();
      }

      foreach (var item in Items)
      {
        (item as ICommandListener)?.DataReceived(channel, data);
        (item as IListener<IControllerCommand>)?.DataReceived(channel, data.OfType<IControllerCommand>().ToList());
      }
    }
  }
}
