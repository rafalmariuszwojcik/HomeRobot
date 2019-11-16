namespace RobotControl.Simulation
{
  using System.Collections.Generic;

  public class Route
  {
    private readonly SimulationPoint start;
    private readonly List<Movement> movements = new List<Movement>();

    public Route(double x, double y, double angle)
    {
      start = new SimulationPoint(x, y, angle);
    }

    public SimulationPoint Start
    {
      get { return start; }
    }

    public IList<Movement> Movements
    {
      get { return movements/*.AsReadOnly()*/; }
    }
  }
}