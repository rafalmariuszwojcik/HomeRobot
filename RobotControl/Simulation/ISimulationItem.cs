namespace RobotControl.Simulation
{
  public interface ISimulationItem
  {
    ISimulation Simulation { get; }
    bool StateChanged { get; }
    void ResetState();
  }
}
