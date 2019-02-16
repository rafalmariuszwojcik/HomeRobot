using RobotControl.Core;
using System;
using System.Collections.Generic;

namespace RobotControl.Simulation
{
  public class SimulationListener : ListenerBase<ISimulation>, IListener<ISimulation>
  {
    public SimulationListener(Action<IEnumerable<ISimulation>> action) : base(action)
    {
      SimulationManager.Instance.RegisterListener(this);
    }

    protected override void Dispose(bool disposing)
    {
      SimulationManager.Instance.UnregisterListener(this);
    }
  }
}
