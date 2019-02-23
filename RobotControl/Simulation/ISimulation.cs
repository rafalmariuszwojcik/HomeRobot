using System;
using System.Collections.Generic;

namespace RobotControl.Simulation
{
  public interface ISimulation
  {
    IList<ISimulationItem> Items { get; }
    ISimulationArea SimulationArea { get; }
    bool StateChanged { get; }
    event EventHandler OnStateChanged;
  }
}
