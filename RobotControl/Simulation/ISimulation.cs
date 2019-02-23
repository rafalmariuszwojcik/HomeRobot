using System.Collections.Generic;

namespace RobotControl.Simulation
{
  public interface ISimulation : ISimulationItem
  {
    ISimulationArea SimulationArea { get; }
  }
}