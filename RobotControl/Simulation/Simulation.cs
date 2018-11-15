using System.Collections.Generic;

namespace RobotControl.Simulation
{
  public class Simulation : ISimulation
  {
    public IList<ISimulationItem> Items { get; } = new List<ISimulationItem>();
    public ISimulationArea SimulationArea => new SimulationArea(new Length(1, MeasurementUnit.Meter), new Length(1, MeasurementUnit.Meter));
  }
}
