using RobotControl.Command;
using RobotControl.Communication;
using RobotControl.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotControl.Simulation
{
  public class Simulation : ISimulation, ICommandListener
  {
    public IList<ISimulationItem> Items { get; } = new List<ISimulationItem>();
    public ISimulationArea SimulationArea => new SimulationArea(new Length(10, MeasurementUnit.Meter), new Length(1, MeasurementUnit.Meter));
    public bool StateChanged { get; private set; }
    public event EventHandler OnStateChanged;

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

      StateChanged = Items.Any(x => x.StateChanged);
    }
  }
}
