namespace RobotControl.Simulation
{
  public interface ISimulation : ISimulationItem
  {
    ISimulationArea SimulationArea { get; }
  }
}