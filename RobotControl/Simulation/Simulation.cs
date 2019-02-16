using RobotControl.Command;
using RobotControl.Communication;
using RobotControl.Core;
using System.Collections.Generic;

namespace RobotControl.Simulation
{
  public class Simulation : ISimulation, ICommandListener
  {
    public IList<ISimulationItem> Items { get; } = new List<ISimulationItem>();
    public ISimulationArea SimulationArea => new SimulationArea(new Length(1, MeasurementUnit.Meter), new Length(1, MeasurementUnit.Meter));
    public bool StateChanged { get; private set; }

    void IListener<ICommand>.DataReceived(IChannel channel, IEnumerable<ICommand> data)
    {
      StateChanged = false;
      foreach (var item in Items)
      {
        (item as ICommandListener)?.DataReceived(channel, data);
      }
           
      StateChanged = true;
    }
  }
}
