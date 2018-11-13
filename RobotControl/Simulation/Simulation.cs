using System.Collections.Generic;

namespace RobotControl.Simulation
{
  public class Simulation : ISimulation
  {
    public IList<ISimulationItem> Items { get; } = new List<ISimulationItem>();
  }
}
