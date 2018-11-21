namespace RobotControl.Simulation
{
  public class SimulationArea : ISimulationArea
  {
    public SimulationArea(Length w, Length l)
    {
      Area = new Rect2D(w.Value, l.Value, w.Unit);
    }

    public Rect2D Area { get; }
  }
}
