using RobotControl.Simulation;

namespace RobotControl.Windows
{
  public class SimulationPackage : DataPackage
  {
    public SimulationPackage(ISimulation simulation)
    {
      Simulation = simulation;
    }

    public ISimulation Simulation { get; }
  }
}
