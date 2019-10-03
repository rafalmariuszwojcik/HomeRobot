namespace RobotControl.Simulation
{
  public class SimulationPoint
  {
    public SimulationPoint(double x, double y, double angle) 
    {
      X = x;
      Y = y;
      Angle = angle;
    }

    public double X { get; set; }

    public double Y { get; set; }

    public double Angle { get; set; }
  }
}
